using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettySocket
{
    public class HeartBeatHandler : ChannelHandlerAdapter
    {
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent) {
                IdleState idleState = ((IdleStateEvent)evt).State;
                switch (idleState)
                {
                    case IdleState.ReaderIdle:
                        context.CloseAsync();
                        break;
                    case IdleState.WriterIdle:
                        context.Channel.WriteAndFlushAsync("asdasd$");
                        break;
                    case IdleState.AllIdle:
                        break;
                    default:
                        break;
                }
            } else {
                try
                {
                    base.UserEventTriggered(context, evt);
                }
                catch (Exception e)
                {
                    //e.printStackTrace();
                }
            }
        }
    }
}
