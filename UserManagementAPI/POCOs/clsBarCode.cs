#nullable disable
using IronBarCode;
using SixLabors.ImageSharp.Memory;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

namespace UserManagementAPI.POCOs
{
    public class clsBarCode
    {
        public clsBarCode()
        {
            
        }

        #region methods

        public async Task<byte[]> returnZippedBarCodesAsync(string bcode, string annotate, string itemDescr,string folder)
        {
            var tempName = Path.Combine(utils.ConfigObject.ZIPPED_FOLDER, $"{annotate}");

            try
            {
                if (Directory.Exists(tempName))
                {
                    Directory.Delete(tempName,true);
                    Directory.CreateDirectory(tempName);
                }
                else { Directory.CreateDirectory(tempName); }

                var barcodes = bcode.Split('|');
                var itemD = itemDescr.Split(',');

                using (MemoryStream ms = new MemoryStream())
                {
                    using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        //get all images
                        foreach (var item in barcodes)
                        {
                            var barcode = BarcodeWriter.CreateBarcode(item, BarcodeEncoding.Code128);
                            barcode.AddAnnotationTextAboveBarcode(annotate);

                            var im = barcode.Image;
                            var data = barcode.ToPngBinaryData();
                            await ImageFromByteArray(data, Path.Combine($"{tempName}",$"{item}.png"));

                            //add to zip file
                            //var entry = zip.CreateEntry(item);
                            var entry = zip.CreateEntry(Path.Combine($"{tempName}", $"{item}.png"));

                            using (var fileStream = new MemoryStream(data))
                            using (var entryStream = entry.Open())
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }

                    return ms.ToArray();
                }
            }
            catch(Exception x)
            {
                return Array.Empty<byte>();
            }
        }

        public async Task<byte[]> returnBarCodeAsync(string bcode,string annotate, string itemDescr)
        {
            try
            {
                var barcode = BarcodeWriter.CreateBarcode(bcode, BarcodeEncoding.Code128);
                barcode.AddAnnotationTextAboveBarcode(annotate);

                var im = barcode.Image;
                byte[] barcodeBytes = barcode.ToPngBinaryData();

                //ConvertImageToBytes((Image)im);

                return barcodeBytes;
            }
            catch(Exception x)
            {
                return Array.Empty<byte>();
            }            
        }

        public  async Task ImageFromByteArray(byte[] bytes, string filename)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (Image image = Image.FromStream(ms,true,true))
            {
                image.Save(filename, ImageFormat.Png);
            }
        }

        public byte[] ConvertImageToBytes(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        #endregion

    }
}
