using festivalModel;

namespace festivalServices
{

    public interface IFestivalObserver
    {
        void soldTicket(Ticket ticket);
    }
}