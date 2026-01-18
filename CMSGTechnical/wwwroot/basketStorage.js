window.basketStorage = (function () {
    let key = null;
    let dotnet = null;

    function init(dotnetRef, storageKey) {
        dotnet = dotnetRef;
        key = storageKey;

        window.addEventListener("storage", function (e) {
            if (!e) return;
            if (e.key !== key) return;

            // notify Blazor that another tab changed the basket
            if (dotnet) {
                dotnet.invokeMethodAsync("NotifyExternalChange");
            }
        });
    }

    function save(storageKey, json) {
        localStorage.setItem(storageKey, json);
    }

    function load(storageKey) {
        return localStorage.getItem(storageKey) || "";
    }

    return {
        init: init,
        save: save,
        load: load
    };
})();
