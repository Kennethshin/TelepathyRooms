namespace TelepathyRooms.Helper
{
    public class FloorNumberHelper
    {
        public int GetFloorNumber(string roomNumber)
        {
            int val = 0;
            string number = string.Empty;

            for (int i = 0; i < roomNumber.Length; i++)
            {
                if (char.IsDigit(roomNumber[i]))
                    number += roomNumber[i];
                else
                    break;
            }

            if (number.Length > 0)
                val = int.Parse(number);

            return val;
        }
    }
}
