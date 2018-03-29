using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettySocket
{
    public class ServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine("the client from " + context.Channel.RemoteAddress + "connected success !!!");
            context.Channel.WriteAndFlushAsync("Welcome"+'$');
        }
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("the client from " +context.Channel.RemoteAddress + "lost connected !!!");
        }
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Console.WriteLine(message as string);
          //  Console.WriteLine()
            //var data =message as string;
            //if (data != null)
            //{
            //    Console.WriteLine(data);
            //    if (data.Equals("PING"))
            //    {
            //        context.Channel.WriteAndFlushAsync("PONG"+Util.SocketUtil.AsciiKey);
            //    }
            //}
        }
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            Console.WriteLine("channel 通道 Read 读取 Complete 完成");
            context.Flush();
            //base.ChannelReadComplete(context);
        }
    }
}
