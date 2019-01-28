using Discord.Audio;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EynwaDiscordBot.Functions
{
    public class SendAudio
    {
        public SendAudio()
        {}

        private Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                //Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                Arguments = $"-hide_banner -loglevel panic -i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(ffmpeg);
        }

        public async Task SendAsync(IAudioClient client, string path)
        {
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }
    }
}
