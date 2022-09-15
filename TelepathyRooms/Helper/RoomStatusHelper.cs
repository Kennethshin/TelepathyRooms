namespace TelepathyRooms.Helper
{
    public class RoomStatusHelper
    {
        public string RoomStatusHandler(string roomStatus, string action)
        {
            if (string.IsNullOrEmpty(roomStatus)) 
                return RoomStatus.None;

            switch (roomStatus)
            {
                case RoomStatus.Available:
                    return AvailableStatusHandler(action);
                case RoomStatus.Occupied:
                    return OccupiedStatusHandler(action);
                case RoomStatus.Vacant:
                    return VacantStatusHandler(action);
                case RoomStatus.Repair:
                    return RepairStatusHandler(action);
                default:
                    return RoomStatus.None;
            }
        }

        private string AvailableStatusHandler(string action)
        {
            if (action == RoomActions.CheckIn)
                return RoomStatus.Occupied;
            else return RoomStatus.None;
        }

        private string OccupiedStatusHandler(string action)
        {
            if (action == RoomActions.CheckOut)
                return RoomStatus.Vacant;
            else return RoomStatus.None;
        }

        private string VacantStatusHandler(string action)
        {
            if (action == RoomActions.Clean)
                return RoomStatus.Available;
            else if (action == RoomActions.OutOfService)
                return RoomStatus.Repair;
            else return RoomStatus.None;
        }

        private string RepairStatusHandler(string action)
        {
            if (action == RoomActions.Repaired)
                return RoomStatus.Vacant;
            else return RoomStatus.None;
        }
    }
}
