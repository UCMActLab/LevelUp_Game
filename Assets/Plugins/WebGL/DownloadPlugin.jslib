mergeInto(LibraryManager.library, {
    DownloadFile: function(filenamePtr, base64Ptr) {
        // Convertir los punteros de memoria de Unity a strings de JavaScript
        var filename = UTF8ToString(filenamePtr);
        var base64 = UTF8ToString(base64Ptr);

        // Crear un enlace HTML invisible
        var link = document.createElement('a');
        link.download = filename;
        // Asignar los datos de la imagen en formato Base64
        link.href = 'data:image/png;base64,' + base64;
        
        // Simular un clic para iniciar la descarga
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
});