window.addKeyListener = (dotNetRef) => {
    document.addEventListener("keydown", (event) => {
        dotNetRef.invokeMethodAsync("ChangeDirection", event.key);
    });
};