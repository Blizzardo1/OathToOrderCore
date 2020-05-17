#if TESTING
using System.Linq;
#else
using System.Net;
#endif
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using log4net;

namespace OathToOrder {
    internal static class Program {
        private static Config _config;

        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

    #if TESTING
        private static ImageCodecInfo GetEncoderInfo(ImageFormat imgFmt) =>
            ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == imgFmt.Guid);
    #endif


        public static async Task Main(string[] args) {
        #if TESTING
            NativeMethods.AllocConsole();
        #endif

            Log.Info("Begin Program");

            // _log.Info("Load Config");
            _config = await Config.LoadAsync();
        #if TESTING
            IFlurlRequest imgUrl = _config.CameraIp.AppendPathSegments($"api/{_config.ApiVersion}", "snapshot20")
                                          .WithHeaders(new {Content_Type = "application/json"});
        #else
            IFlurlRequest imgUrl = _config.CameraIp.AppendPathSegments($"api/{_config.ApiVersion}", "snapshot")
                                          .WithHeaders(new {Content_Type = "application/json"});
        #endif
            try {
                Log.Info("Obtain Image");
                Stream stream = await imgUrl.PostJsonAsync(new {username = _config.User, password = _config.Password})
                                            .ReceiveStream();

                Image img = Image.FromStream(stream);
                double imgSz = stream.Length.ToKilobyte();
                const string image = "image.jpg";
            #if TESTING
            #endif
                img.Save(image, ImageFormat.Jpeg);

                Log.Info($"Image Ok! {imgSz} KB");

            #if TESTING
                _log.Info("Skip FTP Upload");
            #else
                using var client = new WebClient
                    {Credentials = new NetworkCredential(_config.WeatherUndergroundId, _config.WeatherUndergroundKey)};
                string uploadUri = _config.WeatherUndergroundFtpUri.AppendPathSegment(image);

                Log.Info("Upload to FTP");
                client.UploadFile(new Uri(uploadUri), WebRequestMethods.Ftp.UploadFile, image);
                Log.Info("Upload Ok!");
            #endif
            }
            catch (FlurlHttpException fhe) {
                Log.Error($"Image not ok! {fhe.Message}");
            }
            catch (IOException ioe) {
                Log.Error($"Upload not ok! {ioe.Message}");
            }
            catch (Exception ex) {
                Log.Error("Image or Upload Not Ok!", ex);
            }
        #if TESTING
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(false);
            NativeMethods.FreeConsole();
        #endif
        }
    }
}