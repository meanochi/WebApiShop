
// ----Update----
const updateButton = document.querySelector(".updatebutton")
updateButton.addEventListener("click", (event) => {
    const name = document.querySelector("#username").value
    const pass = document.querySelector("#password").value
    const fname = document.querySelector("#firstname").value
    const lname = document.querySelector("#lastname").value
    const user = {
        UserName: name,
        Password: pass,
        FirstName: fname,
        LastName: lname,
        UserID: sessionStorage.getItem("userID")
    }
    update(user)
})
async function update(user) {
    // Check password strength before submitting
    const response1 = await fetch('api/Password', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user.Password)
    });
    if (response1.ok) {
        const data = await response1.json();
        if (data.strength < 2) {
            alert(`❗Your password is too weak (score: ${data.strength}/4). Please choose a stronger password.`);
            return;
        }
    }

    const id = sessionStorage.getItem("userID")
    const response = await fetch(`api/Users/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    });
    const data = await response;
    console.log('PUT Data:', data);
    if (response.status == 200) {
        sessionStorage.setItem("user", user.FirstName);
        alert("User's details updated successfully ✔️")
        window.location.href = "UserDetails.html";
    }
    else if (response.status == 400) {
        alert(`❗Your password is too weak`)
    }
    else {
        alert(`❌ Error occured. status ${response.status}`)
    }

}

// ----Username----
const hello = document.querySelector("#hello")
hello.textContent = "🎉🤩 Hello " + sessionStorage.getItem("user") + "!! 🤩🎉"

// ----Show----
const form = document.querySelector(".update")
const openForm = document.querySelector("#update")
openForm.addEventListener("click", (event) => {
    form.style.display = "flex"
})


