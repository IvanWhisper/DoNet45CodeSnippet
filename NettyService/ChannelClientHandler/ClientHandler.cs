using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyService.ChannelClientHandler
{
    public class ClientHandler: ChannelHandlerAdapter
    {
        private NettyClient _nettyClient;
        public ClientHandler()
        {

        }
        public ClientHandler(NettyClient nettyClient)
        {
            this._nettyClient = nettyClient;
        }
        public override void ChannelActive(IChannelHandlerContext context)
        {
            context.WriteAndFlushAsync(Util.SocketUtil.PackMsg("IA:CMMP"));
        }
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            //_nettyClient.doConnect().Wait(1000);
        }
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            //Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
            //base.ExceptionCaught(context, exception);
        }
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
            //base.ChannelReadComplete(context);
        }
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            //base.ChannelRead(context, message);
            var str = message as string;
            var data = Util.SocketUtil.BareMsg(str);
            if (!string.IsNullOrEmpty(data))
            {
                if (data.Equals("PING"))
                {
                    context.Channel.WriteAndFlushAsync(Util.SocketUtil.PackMsg("PONG"));
                }
                Console.WriteLine(str);
            }
        }
    }
}
