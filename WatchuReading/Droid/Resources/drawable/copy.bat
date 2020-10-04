robocopy "C:\Users\chamilton\Dropbox\Files etc\app" "Y:\Projects\WatchuReading\Droid\Resources\drawable" 
robocopy "C:\Users\chamilton\Dropbox\Files etc\app" "Y:\Projects\WatchuReading\Droid\Resources\drawable-hdpi"
robocopy "C:\Users\chamilton\Dropbox\Files etc\app" "Y:\Projects\WatchuReading\Droid\Resources\drawable-xhdpi" 
robocopy "C:\Users\chamilton\Dropbox\Files etc\app" "Y:\Projects\WatchuReading\Droid\Resources\drawable-xxhdpi"

robocopy "C:\Users\chamilton\Dropbox\Files etc\app" "Y:\Projects\WatchuReading\iOS\Resources" /E /PURGE /MT /W:10 /XF *.pdb


PAUSE
//EXIT