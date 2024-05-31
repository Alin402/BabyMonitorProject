import subprocess
import asyncio


async def restart_device():
    try:
        print("Starting Livestream...")
        process = await asyncio.create_subprocess_shell(
            "sudo reboot",
            stdout=asyncio.subprocess.PIPE,
            stderr=asyncio.subprocess.PIPE
        )

        await process.communicate()

    except Exception as e:
        print("Exception in livestream: " + str(e))
