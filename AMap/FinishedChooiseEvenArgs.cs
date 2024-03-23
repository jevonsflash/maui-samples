namespace AMap
{
    public class FinishedChooiseEvenArgs
    {

        public FinishedChooiseEvenArgs(string address, Location.Location location)
        {
            Address=address;
            Location=location;
        }
        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
            }
        }


        private Location.Location _location;

        public Location.Location Location
        {
            get { return _location; }
            set
            {
                _location = value;
            }
        }
    }
}