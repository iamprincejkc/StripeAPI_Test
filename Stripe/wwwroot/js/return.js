initialize();

async function initialize() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const sessionId = urlParams.get('session_id');
    const response = await fetch(`https://localhost:7144/api/stripeapi/SessionStatus?session_id=${sessionId}`);
    const session = await response.json();
    console.log(session);

    if (session.status == 'open') {
        window.location.replace('https://localhost:7144/stripe')
    } else if (session.status == 'complete') {
        window.location.replace('https://localhost:7144/confirmation.html')
    }
}