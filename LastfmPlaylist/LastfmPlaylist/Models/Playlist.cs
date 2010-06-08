using System;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LastfmPlaylist.Helpers;

using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;




// todo: make into a webservice
namespace LastfmPlaylist.Models
{
    public static class Playlist
    {
        public static string Text { get; set; }
        public static string Output { get; set; }
        public static bool YouTubeLink { get; set; }
        public static bool WikipediaLink { get; set; }
        public static bool MySpaceLink { get; set; }

        private const int ArtistIndex = 1; 
        private const int TitleIndex = 2;
        //private const int YouTubeIndex = 3;
        //private const int WikipediaIndex = 4;
        //private const int MySpaceIndex = 5;
        
        private static IList<string> TagList { get; set; }
        public static string Convert(string contents)
        {
            Text = contents;

            // m3u8 files support unicode8
            StringBuilder stringBuilder = new StringBuilder();

            StringReader stringReader = new StringReader(contents);

            //read first (useless) line  // #EXTM3U
            stringReader.ReadLine();

            //M3U FORMAT
            //      #EXTINF:<length (sec)>,<artist> - <title>
            //      <absolute path including filename>    // backslashes (\)

            //      #EXTINF:431,Rush - Working Man
            //      D:\Classic rock\Rush\Rush - The Spirit of Radio - Greatest Hits\01.Working Man.mp3

            // the nth song in the m3u playlist is the (nth line in the file/ 2)
            int trackNum = 0;
            string line = stringReader.ReadLine();
            
            while (line != null)
            {
                trackNum++;
                // read id3 tags - length, artist, song
                string songInfo = line;
                TagList = ParseSongInfo(songInfo);
                stringReader.ReadLine();  // ignore filepath
                string outputLine = GetSongLine(trackNum);
                stringBuilder.AppendLine(outputLine);
                line = stringReader.ReadLine();
            }

            stringReader.Close();
            string attributionText =
                @"[size=7]Created using [url=http://www.thegabrielflores.com/playlist-to-lastfm]Playlist to Last.fm[/url][/size]";
            stringBuilder.AppendLine(Environment.NewLine + Environment.NewLine + attributionText);
            
            Output = stringBuilder.ToString();
            return Output;
        }

        // gets converted lastfm line for one song
        private static string GetSongLine(int trackNum)
        {
            
            string outputLine = trackNum + ". " + "[artist]" + TagList[ArtistIndex] + "[/artist] - \"[track artist=" +
                                TagList[ArtistIndex] + "]"
                                + TagList[TitleIndex] + "[/track]\"";
            if (WikipediaLink)
                outputLine += " [size=7]([url=" + GetWikipediaLink(TagList[ArtistIndex]) + "]Wikipedia[/url])[/size]";
            if (MySpaceLink)
                outputLine += " [size=7]([url=" + GetMyspaceLink(TagList[ArtistIndex]) + "]MySpace[/url])[/size]";
            if (YouTubeLink)
            {
                string youtubeLinkString = GetYoutubeLink(TagList[ArtistIndex], TagList[TitleIndex]);
                if (!string.IsNullOrEmpty(youtubeLinkString))
                    outputLine += Environment.NewLine + "[youtube]" + GetYoutubeLink(TagList[ArtistIndex], TagList[TitleIndex]) + "[/youtube]";
            }

            outputLine += Environment.NewLine;
            return outputLine;
            //sb.AppendLine(outputLine);
            //Output = sb.ToString();
        }

        //M3U FORMAT
        //      #EXTINF:<length (sec)>,<artist> - <title>
        //      <absolute path including filename>    // backslashes (\)

        //      #EXTINF:431,Rush - Working Man
        //      D:\Classic rock\Rush\Rush - The Spirit of Radio - Greatest Hits\01.Working Man.mp3

        // result 1. [artist]myartist[/artist] - "[track artist=myartist]mysongtitle[/track]"
        // private const int Fields = 4; // extinf, length, artist, title  // not necessary?
        public static List<string> ParseSongInfo(string songInfo)
        {
            List<string> songInfoList = new List<string>();
            
            try
            {
                // get length           
                songInfo = songInfo.Remove(0, 8); // remove #extinf prefix
                int separatorIndex = songInfo.IndexOf(',');
                string lengthSec = songInfo.Substring(0, separatorIndex);

                // get artist
                songInfo = songInfo.Remove(0, separatorIndex + 1);
                separatorIndex = songInfo.IndexOf(" - ");
                string artist = songInfo.Substring(0, separatorIndex);

                // get title
                songInfo = songInfo.Remove(0, separatorIndex + 3);
                string title = songInfo;


                songInfoList.Add(lengthSec);
                songInfoList.Add(artist);
                songInfoList.Add(title);

                //if (YouTubeLink)
                //    songInfoList.Add(GetYoutubeLink(artist, title));
                //if (WikipediaLink)
                //    songInfoList.Add(GetWikipediaLink(artist));
                //if (MySpaceLink)
                //    songInfoList.Add(GetMyspaceLink(artist));
                

            }
            catch (Exception)
            {
                Console.Write("Error");
                TagList.Add("Error");
                TagList.Add("Error");
                TagList.Add("Error");
            }
            return songInfoList;
            
        }

        

        private static string GetMyspaceLink(string artist)
        {
            string url = @"http://www.google.com/search?q=" + artist + "%20site:myspace.com&btnI";

            return url;
        }

        private static string GetWikipediaLink(string artist)
        {
            string url = @"http://en.wikipedia.org/wiki/" + artist;

            return url;
        }

        private static string GetYoutubeLink(string artist, string title)
        {
            string videoUrl;
            try
            {
            YouTubeRequestSettings requestSettings = new YouTubeRequestSettings(Configuration.GDataApp, Configuration.DeveloperKey);
                YouTubeRequest youTubeRequest = new YouTubeRequest(requestSettings);
            
            YouTubeQuery youTubeQuery = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
            youTubeQuery.Query = artist + " " + title;
            //youTubeQuery.OrderBy = "viewCount";  use default of relevance instead
            youTubeQuery.NumberToRetrieve = 1;

            Feed<Video> videoFeed = youTubeRequest.Get<Video>(youTubeQuery);
            
            if (videoFeed.TotalResults == 0)
            {
                return "";
            }

            videoUrl = "http://www.youtube.com/watch?v=";
            foreach (Video vid in videoFeed.Entries)
            {
                videoUrl += vid.VideoId;
            }
            }
            catch (Exception)
            {
                videoUrl = "";
            }
         
            return videoUrl;
        }

    }
}
