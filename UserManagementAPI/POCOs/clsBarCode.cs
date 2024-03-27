#nullable disable
using IronBarCode;
using System.Drawing;
using System.IO;

namespace UserManagementAPI.POCOs
{
    public class clsBarCode
    {
        public clsBarCode()
        {
            
        }

        #region methods

        public async Task<byte[]> returnBarCodeAsync(string txt,string annotate)
        {
            try
            {
                //TODO: returns bar code
                var barcode = BarcodeWriter.CreateBarcode(txt, BarcodeEncoding.Code128);
                barcode.AddAnnotationTextAboveBarcode(annotate);

                byte[] barcodeBytes = barcode.ToPngBinaryData();
                return barcodeBytes;
            }
            catch(Exception x)
            {
                return Array.Empty<byte>();
            }
            
        }

        #endregion

    }
}
