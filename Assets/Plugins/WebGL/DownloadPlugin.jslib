mergeInto(LibraryManager.library, {
    DownloadFileRaw: function(filenamePtr, arrayPtr, size) {
        var filename = UTF8ToString(filenamePtr);
        
        // Creamos una referencia directa a los bytes dentro del HEAP de Unity
        var bytes = new Uint8Array(HEAPU8.buffer, arrayPtr, size);

        // Generamos un objeto Blob nativo del navegador
        var blob = new Blob([bytes], { type: 'image/png' });
        var url = URL.createObjectURL(blob);

        // Forzamos la descarga mediante el DOM
        var link = document.createElement('a');
        link.download = filename;
        link.href = url;
        
        document.body.appendChild(link);
        link.click();
        
        // Limpieza inmediata de la memoria del navegador
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }
});