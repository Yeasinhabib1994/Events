using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            var video = new Video() { Title = "video 1" };
            var videoEncoder = new VideoEncoder();
            var mailService = new MailService();
            var messageService = new MessageService();

            videoEncoder.VideoEncoded += mailService.OnVideoEncoded;
            videoEncoder.VideoEncoded += messageService.OnVideoEncoded;
            videoEncoder.Encode(video);
        }
    }

    public class Video
    {
        public string Title { get; set; }
    }

    public class VideoEncoder
    {
        //public delegate void VideoEncodedEventHandler(object source, VideoEventArgs e);// 1 - define a delegate
        //public event VideoEncodedEventHandler VideoEncoded;// 2 - define an event based on that delegate
        public event EventHandler<VideoEventArgs> VideoEncoded; // this is equivalent to step 1 & 2
        public void Encode(Video video)
        {
            Console.WriteLine("Encoding video...");
            Thread.Sleep(3000);

            OnVideoEncoded(video);
        }

        protected virtual void OnVideoEncoded(Video video)// 3 - raise that event
        {
            if (VideoEncoded != null)
                VideoEncoded(this, new VideoEventArgs() { Video = video });// this method is the source, and no additional EventArgs data is needed
        }
    }

    public class MailService
    {
        public void OnVideoEncoded(object source, VideoEventArgs e)
        {
            Console.WriteLine("MailService: sending an email..." + e.Video.Title);
        }
    }

    public class MessageService
    {
        public void OnVideoEncoded(object source, VideoEventArgs e)
        {
            Console.WriteLine("MessageService: sending a text message..." + e.Video.Title);
        }
    }

    public class VideoEventArgs : EventArgs
    {
        public Video Video { get; set; }
    }
}
