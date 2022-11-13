export function getTokenizedElement(token)
{
    return document.querySelector(`[data-blazor-token="${token}"]`)
}