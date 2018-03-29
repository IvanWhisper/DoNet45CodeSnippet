using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NettyService.ChannelClientHandler
{
    public class HeartBeatHandler: ChannelHandlerAdapter
    {
        private NettyClient _nettyClient;
        public HeartBeatHandler()
        {

        }
        public HeartBeatHandler(NettyClient nettyClient)
        {
            this._nettyClient = nettyClient;
        }
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if(evt is IdleStateEvent)
            {
                IdleState idleState = ((IdleStateEvent)evt).State;
                switch (idleState)
                {
                    case IdleState.ReaderIdle:
                        //Console.WriteLine("读超时");
                        ////context.Channel.CloseAsync();
                        //_nettyClient.doConnect().Wait(1000);
                        break;
                    case IdleState.WriterIdle:
                        context.WriteAndFlushAsync("PING"+Util.SocketUtil.AsciiKey);
                        break;
                    case IdleState.AllIdle:
                        context.WriteAndFlushAsync("PING" + Util.SocketUtil.AsciiKey);
                        break;
                }

            }
            //base.UserEventTriggered(context, evt);
        }
    }
}
