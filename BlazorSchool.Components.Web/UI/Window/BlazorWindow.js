import { getTokenizedElement } from "/_content/BlazorSchool.Components.Web/BlazorTokenize.min.js"

let handleDragOverReference = null;

function handleDragOver(mouseEvent, windowToken)
{
    mouseEvent = mouseEvent || window.event;
    let element = getTokenizedElement(windowToken);
    let rect = element.getBoundingClientRect();
    let maxLeft = window.innerWidth - rect.width;
    let maxTop = window.innerHeight - rect.height;

    if (mouseEvent.pageX < maxLeft)
    {
        element.style.left = `${mouseEvent.pageX}px`;
    }

    if (mouseEvent.pageY < maxTop)
    {
        element.style.top = `${mouseEvent.pageY}px`;
    }

    mouseEvent.preventDefault();
}

function registerDocumentMouseMoveEvent(windowToken)
{
    handleDragOverReference = (mouseEvent) => handleDragOver(mouseEvent, windowToken);
    document.addEventListener("dragover", handleDragOverReference, false);
}

function unregisterDocumentMouseMoveEvent()
{
    document.removeEventListener("dragover", handleDragOverReference, false);
}

export function registerWindowTitleEvent(windowTitleToken, windowToken)
{
    let element = getTokenizedElement(windowTitleToken);

    let handleDragStartReference = (e) =>
    {
        e.dataTransfer.effectAllowed = "move";
        e.dataTransfer.setDragImage(new Image(), 0, 0);
        registerDocumentMouseMoveEvent(windowToken);
    };

    element.addEventListener("dragstart", handleDragStartReference, false);
    element.addEventListener("dragend", unregisterDocumentMouseMoveEvent, false);
}