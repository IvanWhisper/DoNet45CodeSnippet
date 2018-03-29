using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyService.ChannelClientHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NettyService
{
    public class NettyClient
    {
        /**
* 读超时
*/
        private static int READ_IDEL_TIME_OUT = 15;
        /**
         * 写超时
         */
        private static int WRITE_IDEL_TIME_OUT = 10;
        /**
         * 所有超时
         */
        private static int ALL_IDEL_TIME_OUT = 20;
        /**
         * 传输消息最大长度
         */
        private string _IP="127.0.0.1";
        private int _Port=8999;
        private string _dataHead=Util.SocketUtil.Ascii2;
        private string _dataFoot=Util.SocketUtil.Ascii3;
        private const int MAX_FRAME_LENGTH = 3 * 1024;

        private MultithreadEventLoopGroup group = new MultithreadEventLoopGroup();
        private IChannel clientChannel;
        private Bootstrap bootstrap;
        public string IP
        {
            get
            {
                return _IP;
            }

            set
            {
                _IP = value;
            }
        }

        public int Port
        {
            get
            {
                return _Port;
            }

            set
            {
                _Port = value;
            }
        }

        public string DataHead
        {
            get
            {
                return _dataHead;
            }

            set
            {
                _dataHead = value;
            }
        }

        public string DataFoot
        {
            get
            {
                return _dataFoot;
            }

            set
            {
                _dataFoot = value;
            }
        }

        public NettyClient()
        {
        }
        public async Task RunClientAsync()
        {
            try
            {
                bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new IdleStateHandler(READ_IDEL_TIME_OUT, WRITE_IDEL_TIME_OUT, ALL_IDEL_TIME_OUT));
                        pipeline.AddLast(new HeartBeatHandler(this));
                        pipeline.AddLast(new DelimiterBasedFrameDecoder(MAX_FRAME_LENGTH, false, false, Unpooled.CopiedBuffer(Encoding.ASCII.GetBytes(Util.SocketUtil.Ascii3))));
                        pipeline.AddLast("framing-enc", new StringEncoder(Encoding.UTF8));
                        pipeline.AddLast("framing-dec", new StringDecoder(Encoding.UTF8));
                        pipeline.AddLast("echo", new ClientHandler(this));
                    }));
                await doConnect();
                //clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(IP), Port));
                Console.ReadLine();
                await clientChannel.CloseAsync();
                //clientChannel.wr
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }
        public async Task doConnect()
        {
            if (clientChannel != null)
            {
                if (clientChannel.Active)
                {
                    Console.WriteLine("关闭通道");
                    await clientChannel.CloseAsync();
                }
            }
            Thread.Sleep(1000);
            clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(IP), Port));
        }
    }
}
