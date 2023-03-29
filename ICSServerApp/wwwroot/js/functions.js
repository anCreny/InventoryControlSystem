async function logout(){
    const response = await fetch("Api/Logout", {
        method: "POST"
    })
    
    if (response.ok === true){
        location.reload();
    }
}