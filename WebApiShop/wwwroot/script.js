
const loginButton = document.querySelector(".loginbutton")
const signUpButton = document.querySelector(".signupbutton")

// ----Login----
async function login(user) {
    const response = await fetch('api/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    });
    const data = await response;
    console.log('POST Data:', data);
    if (response.status == 200) {
        const dataObj = await response.json();
        sessionStorage.setItem("user", dataObj.firstName)
        sessionStorage.setItem("userID", dataObj.id)
        window.location.href = "UserDetails.html"
    }
    else {
        alert(`Error occured. status ${response.status}`)
    }
}
loginButton.addEventListener("click", (event) => {
    const name = document.querySelector("#username1").value
    const pass = document.querySelector("#password1").value
    const user = {
        UserName: name,
        Password: pass,
    }
    login(user)

})

// ----Signup----
async function signUp(user) {
    // Check password strength before submitting
    const passwordStrength = await checkPassword(user.Password);
    if (passwordStrength * 4 < 2) {
        alert(`❗Your password is too weak (score: ${Math.round(passwordStrength * 4)}/4). Please choose a stronger password.`);
        return;
    }

    const response = await fetch('api/Users', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    });
    const data = await response;
    console.log('POST Data:', data);
    if (response.status == 201) {
        alert(`User added successfully! ☑️\nplease log in to get inside`)
    }
    else if (response.status == 400) {
        alert(`❗Your password is too weak`)
    }
    else {
        alert(`❌ Error occured. status ${response.status}`)
    }

}
signUpButton.addEventListener("click", (event) => {
    const name = document.querySelector("#username2").value
    const pass = document.querySelector("#password2").value
    const fname = document.querySelector("#firstname").value
    const lname = document.querySelector("#lastname").value
    const user = {
        UserName: name,
        Password: pass,
        FirstName: fname,
        LastName: lname
    }
    signUp(user)
})
//-----Check Password Strength
const checkButton = document.querySelector("#checkPassStrength")
checkButton.addEventListener("click", async (event) => {
    const password = document.querySelector("#password2").value
    const strength = await checkPassword(password)
    const progressBar = document.querySelector(".progressBar")
    progressBar.innerHTML = `<progress value =${strength}></progress>`
})

async function checkPassword(password) {
    const response = await fetch('api/Password', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(password)
    });
    const data = await response.json();
    console.log(data)
    if (response.status == 200) {
        console.log(data.strength)
        return data.strength/ 4;
    }
    else {
        return 0;
    }
}
// ----Auto-check password on input----
const password2Input = document.querySelector("#password2")
password2Input.addEventListener("input", async (event) => {
    const password = password2Input.value
    if (password.length > 0) {
        const strength = await checkPassword(password)
        const progressBar = document.querySelector(".progressBar progress")
        progressBar.value = strength
    }
})

// ----Show----
const form = document.querySelector(".signup")
const openForm = document.querySelector("#openform")
openForm.addEventListener("click", (event) => {
    form.style.display = "flex"
})

