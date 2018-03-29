using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettySocket
{
    class Program
    {
        /**
     * 读超时
     */
        private static int READ_IDEL_TIME_OUT = 10;
        /**
         * 写超时
         */
        private static int WRITE_IDEL_TIME_OUT = 5;
        /**
         * 所有超时
         */
        private static int ALL_IDEL_TIME_OUT = 20;
        /**
         * 传输消息最大长度
         */
        private static int MESSAGE_MAX_SIZE = 1024*3;
        static async Task RunServerAsync()
        {

            IEventLoopGroup bossGroup=new MultithreadEventLoopGroup();
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
            ServerBootstrap serverBootstrap = new ServerBootstrap();
            try
            {
                serverBootstrap.Group(bossGroup, workerGroup)
                   .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 128)
                    .ChildOption(ChannelOption.SoKeepalive, true)
                    .ChildOption(ChannelOption.TcpNodelay, true)
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new IdleStateHandler(READ_IDEL_TIME_OUT, WRITE_IDEL_TIME_OUT, ALL_IDEL_TIME_OUT));
                        pipeline.AddLast(new HeartBeatHandler());
                        pipeline.AddLast(new DelimiterBasedFrameDecoder(MESSAGE_MAX_SIZE, true, false,Unpooled.CopiedBuffer(Encoding.ASCII.GetBytes(new char[] {(char)'$'}))));
                        pipeline.AddLast("framing-enc", new StringEncoder(Encoding.UTF8));
                        pipeline.AddLast("framing-dec", new StringDecoder(Encoding.UTF8));
                        pipeline.AddLast("echo", new ServerHandler());
                    }));

                IChannel boundChannel = await serverBootstrap.BindAsync(8999);

                Console.ReadLine();

                await boundChannel.CloseAsync();
            }
            finally
            {
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }

        static void Main() => RunServerAsync().Wait();
    }
}
