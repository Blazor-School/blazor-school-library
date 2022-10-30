let handleDragOverReference = null;

function handleDragOver(e, domId)
{
    e = e || window.event;
    let element = document.getElementById(domId);
    let rect = element.getBoundingClientRect();
    let maxLeft = window.innerWidth - rect.width;
    let maxTop = window.innerHeight - rect.height;

    if (e.pageX < maxLeft)
    {
        element.style.left = `${e.pageX}px`;
    }

    if (e.pageY < maxTop)
    {
        element.style.top = `${e.pageY}px`;
    }

    e.preventDefault();
}

function registerDocumentMouseMoveEvent(windowId)
{
    handleDragOverReference = (e) => handleDragOver(e, windowId);
    document.addEventListener("dragover", handleDragOverReference, false);
}

function unregisterDocumentMouseMoveEvent()
{
    document.removeEventListener("dragover", handleDragOverReference, false);
}

export function registerWindowTitleEvent(windowTitleId, windowId)
{
    let element = document.getElementById(windowTitleId);

    let handleDragStartReference = (e) =>
    {
        e.dataTransfer.effectAllowed = "move";
        e.dataTransfer.setDragImage(new Image(), 0, 0);
        registerDocumentMouseMoveEvent(windowId);
    };

    element.addEventListener("dragstart", handleDragStartReference, false);
    element.addEventListener("dragend", unregisterDocumentMouseMoveEvent, false);
}