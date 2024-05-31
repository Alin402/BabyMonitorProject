import subprocess
import asyncio


async def start_live_stream(event, appData):
    try:
        print("Starting Livestream...")
        streamCommand = "libcamera-vid -t 0 --width 640 --height 362  --inline -o - | ffmpeg -re -ar 44100 -ac 2 -acodec pcm_s16le -f s16le -ac 2 -i /dev/zero -f h264 -i - -vcodec copy -acodec aac -ab 128k -g 50 -strict experimental -f flv " + appData.StreamingChannelUrl
        print(streamCommand)
        process = await asyncio.create_subprocess_shell(
            streamCommand,
            stdout=asyncio.subprocess.PIPE,
            stderr=asyncio.subprocess.PIPE
        )

        await process.communicate()

    except Exception as e:
        print("Exception in livestream: " + str(e))