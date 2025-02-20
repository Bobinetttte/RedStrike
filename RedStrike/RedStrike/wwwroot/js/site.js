window.startListeningToIframeInputs = function () {
    const iframe = document.getElementById("iframeSite");

    // Vérifie si l'iframe existe et si son contenu est accessible
    if (iframe && iframe.contentWindow) {
        console.info("Iframe Existe  dsfd  fsdfdfdf");

        iframe.onload = (event) => {
            console.info("Iframe Load");
            try {
                // Accède à tous les éléments <input> dans l'iframe
                var inputs = iframe.contentWindow.document.querySelectorAll("input");
                var inputData = [];
                console.info("Iframe Suite");

                inputs.forEach(function (input) {
                    // Enregistre chaque input et sa valeur
                    inputData.push({
                        name: input.name,
                        value: input.value,
                        type: input.type
                    });
                });

                // Affiche les données des inputs dans la console
                console.log(inputData);
            } catch (error) {
                console.error("Erreur d'accès aux inputs dans l'iframe : " + error.message);
            }
        };
    } else {
        console.error("L'iframe n'a pas encore été chargée ou elle n'existe pas.");
    }
}