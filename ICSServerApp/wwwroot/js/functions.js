async function logout(){
    const response = await fetch("Api/Logout", {
        method: "POST"
    })
    
    if (response.ok === true){
        location.reload();
    }
}

async function getUser(id){
    const response = await fetch(`/Api/GetUser/?id=${id}`,{
        method: "GET",
        headers: {"Accept" : "json/application"}
    })

    if (response.ok === true){
        return await response.json();
    }
}

function getOffset(element){
    let leftOffsetElement = element;
    
    let offsetLeft = 0;
    do{
        if (!isNaN(leftOffsetElement.offsetLeft)){
            offsetLeft += leftOffsetElement.offsetLeft;
        }
    } while (leftOffsetElement = leftOffsetElement.offsetParent())
    
    let topOffsetElement = element;
    
    let offsetTop = 0;
    do{
        if (!isNaN(topOffsetElement.offsetTop)){
            offsetTop += topOffsetElement.offsetTop;
        }
    } while (topOffsetElement = topOffsetElement.offsetParent());

    return [offsetLeft, offsetTop];
}