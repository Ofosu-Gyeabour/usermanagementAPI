#nullable disable

using KellermanSoftware.CompareNetObjects.TypeComparers;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UserManagementAPI.POCOs
{
    public class clsQRCode
    {
        public clsQRCode()
        {
            
        }

        public clsCollectionAndDelivery data { get; set; }

        #region Methods

        public async Task<byte[]> returnQRCode(string text2Encode)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(text2Encode, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qRCodeData);
            byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);

            return qrCodeAsBitmapByteArr;
        }

        #endregion
    }
}
