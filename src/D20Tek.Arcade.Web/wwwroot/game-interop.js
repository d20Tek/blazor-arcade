window.addKeyListener = (dotNetRef) => {
    document.addEventListener("keydown", (event) => {
        dotNetRef.invokeMethodAsync("HandleKeydown", event.key);
    });
};