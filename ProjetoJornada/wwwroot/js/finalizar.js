

/*EVENTO DO BOTÃO PARA APARECER A OPÇÃO DE PAGAMENTO*/ 

function pagar() {
    document.querySelector(".conteiner_pagamento_checkbox").style.display = "block"
}

const form = document.querySelector('#frmCheckBox')
form.addEventListener('submit', (e) => {
    e.preventDefault()
})



/*EVENTO DO BOTÃO GERAR FECHAR DO MODAL*/ 

function gerar(e) {

    document.querySelector(".secao_finalizar2").style.display = "block"

    
        
    
}

