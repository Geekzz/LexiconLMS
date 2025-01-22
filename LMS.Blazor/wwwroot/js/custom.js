//function downloadFile(fileUrl, fileName)
//{
//    const link = document.createElement('a');
//    link.href = fileUrl;
//    link.download = fileName;
//    document.body.appendChild(link);
//    link.click();
//    document.body.removeChild(link);
//}

function downloadFile(fileData, fileName, contentType) {
    const uint8Array = new Uint8Array(fileData); // Convert .NET byte[] to Uint8Array
    const blob = new Blob([uint8Array], { type: contentType });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url); // Clean up
}