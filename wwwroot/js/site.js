// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function manejarEventoTeclado() {
    var expediente = "";
    document.onkeyup = function(e) {
      e = e || window.event;
      
      expediente += String.fromCharCode(e.keyCode);
      if (e.keyCode == 13) {
        // Acciones a realizar cuando se presiona la tecla Enter
        document.getElementById("evento").innerHTML = (expediente + " Se ha inscrito al evento Hackathon 2025 - ");
        agregarExpediente(expediente);
        
        expediente = "";
      }
    };
  }
  
  // Llamar a la función al cargar la ventana
  window.onload = function() {
    manejarEventoTeclado();
};

function agregarExpediente(expediente){
    console.log(JSON.stringify(expediente.trim()))
    fetch('/Home/AgregarExpediente', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(expediente.trim())
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('No se pudo enviar el expediente')
        }
        return response.json();
    })
    .then(data => {
        console.log('Se envio el expediente correctamente');
    })
    .catch(error => {
        console.error('Error: ', error);
    });
}