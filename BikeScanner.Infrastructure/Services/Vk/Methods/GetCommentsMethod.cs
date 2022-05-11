//using System;

//namespace BikeScanner.Infrastructure.Services.Vk.Methods
//{
//	internal class GetCommentsMethod : VkApiMethod
//	{
//        private int _offset;
//        private int _count;

//        public GetCommentsMethod(string accessToken)
//            : base(accessToken)
//        { }

//        public override string Method => "photos.getComments";

//        public override string Params =>
//            $"owner_id={OwnerId}&photo_id={PhotoId}&&offset={Offset}&count={Count}";

//        public int OwnerId { get; set; }

//        public int PhotoId { get; set; }

//        public int Offset
//        {
//            get => _offset;
//            set
//            {
//                if (value < 0)
//                    throw new ArgumentException($"{nameof(Offset)} must be more than 0");

//                _offset = value;
//            }
//        }

//        public int Count
//        {
//            get => _count;
//            set
//            {
//                if (value < 1 || value > 100)
//                    throw new ArgumentException($"{nameof(Count)} must be in range (1, 100)");

//                _count = value;
//            }
//        }
//    }
//}

