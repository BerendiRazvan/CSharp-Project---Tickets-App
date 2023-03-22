using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace festivalClientForm
{

    public enum FestivalUserEvent
    {
        BuyTicket
    }

    
    public class FestivalUserEventArgs:EventArgs
    {
        private readonly FestivalUserEvent userEvent;
        private readonly Object data;

        public FestivalUserEventArgs(FestivalUserEvent userEvent, object data) {
            this.userEvent = userEvent;
            this.data = data;
        }

        public FestivalUserEvent FestivalEventType
        {
            get { return userEvent; }
        }

        public object Data
        {
            get { return data; }
        }

    }

   

}
