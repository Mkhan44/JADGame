cd "$(dirname "$0")"
echo "splitting jar files…"

jar -xf EssentialKit.jar

echo "Extracted files…"

rm -rf EssentialKit.jar

echo "Packing individual jar files…"

for i in addressbook billingservices cloudservices gameservices deeplinkservices extras mediaservices networkservices notificationservices sharingservices uiviews webview socialauth
do
    echo "Making $i.jar…"
    jar -cf feature.$i.jar com/voxelbusters/android/essentialkit/features/$i
    rm -rf com/voxelbusters/android/essentialkit/features/$i
done

jar -cf essentialkit.core.jar com/voxelbusters/android

rm -rf com
rm -rf META-INF

exit 0