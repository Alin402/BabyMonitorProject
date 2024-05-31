export const readImageAsPath = (imageFile, callback) => {
    try {
        const reader = new FileReader();
        reader.onloadend = function() {
            callback(reader.result);
        }
        if (imageFile) {
            reader.readAsDataURL(imageFile);
        }
    } catch (error) {
        console.log(error);
    }
}