async function existeUsuario(usuario) {
    if (usuario === null || usuario === "" || !isNaN(usuario)) {
        alert('Houve um erro');
    }
    const endPoint = usuario + '/existe';
    const dados = {
        method: 'GET',
        //body: JSON.stringify({userName: usuario }),
        headers: {
            'content-type': 'application/json'
        }
    };
    let req = await fetch(endPoint, dados);
    req.json().then(function (dadosRecebidos) {
        if (dadosRecebidos.existeUsuario === true) {
            document.getElementById('msg').style.color = 'red';
            document.getElementById('msg').innerHTML = 'O usuário ' + usuario + ' não está disponível. Forneça outro nome';
        }
        else {
            document.getElementById('msg').style.color = 'green';
            document.getElementById('msg').innerHTML = 'O usuário ' + usuario + ' está disponível.';
        }
    }).catch(function (err) {
        document.getElementById('msg').a.style.color = 'red';
        document.getElementById('msg').innerHTML = err;
    });
       
    
}

function isIdValid(codigoUnico) {
    const codigosConhecidosInvalidos = ['000000000-00', '1111111111-11', '22222222222-22', '333333333-33', '444444444-44', '555555555-55', '666666666-66'
        ,'777777777-77', '888888888-88', '999999999-99', '012345678-90', '123456789-12'];
    if (codigosConhecidosInvalidos.indexOf(codigoUnico, 0) > - 1) {
        document.getElementById('msg').style.color = 'red';
        document.getElementById('msg').innerHTML = 'O CPF é inválido. Ele consta na lista de CPFs conhecidos inválidos. \n Forneça outro';
        return false;
    }

    let soma = 0;
    codigoUnico = codigoUnico.toString();
    codigoUnico = codigoUnico.replace('.', '').replace('.', '').replace('.', '');
    for (let i = 0; i < 9; i++) {
        soma += parseInt(codigoUnico.charAt(i)) * (10 - i);
    }
    let rev = 11 - (soma % 11);
    if (rev === 10 || rev === 11) {
        rev = 0;
    }
    if (rev != parseInt(codigoUnico.charAt(9))) {
        document.getElementById('msg').style.color = 'red';
        document.getElementById('msg').innerHTML = 'O CPF é inválido. Forneça outro.';
        return false;
    }
    soma = 0;
    for (i = 0; i < 10; i++) {
        soma += parseInt(codigoUnico.charAt(i)) * (11 - i);
    }
    rev = 11 - (soma % 11);
    if (rev === 10 || rev === 11) {
        rev = 0;
    }
    if (rev != parseInt(codigoUnico.charAt(10))) {
        document.getElementById('msg').style.color = 'red';
        document.getElementById('msg').innerHTML = 'O CPF é inválido. Forneça outro.';
        return false;
    }
    return true;
}

//function fadeIn(elmt) {()
//    const pesquisar = elmt;

//}

async function respComentario(idComentario, idUsuario, idProduto) {
    let form = document.createElement('form');
    form.name = "comentario" + idComentario;
    form.id = "comment" + idComentario;
    form.method = 'POST';
    let mensagem = document.createElement('textarea');
    mensagem.name = "Mensagem";
    mensagem.id = "msg";
    mensagem.rows = 30;
    mensagem.cols = 60;
    let foiAprovado = document.createElement('input');
    foiAprovado.type = "checkbox";
    foiAprovado.name = "FoiAprovado";
    foiAprovado.id = "aprovado";
    let notaSelect = document.createElement('select');
    notaSelect.name = "Avaliacao";
    notaSelect.i = "avl";
    let optNota;
    for (let opt = 1; opt <= 5; opt++) {
        optNota = document.createElement('option');
        optNota.innerHTML = opt;
        optNota.value = opt;
        notaSelect.append(optNota);
    }
    let botaoResponderComentario = document.createElement('input');
    botaoResponderComentario.type = "submit";
    botaoResponderComentario.value = "Responder Comentário";
    botaoResponderComentario.id = "rcomment" + idComentario;
    let botaoFecharForm = document.createElement('button');
    botaoFecharForm.id = "fcomment" + idComentario;
    botaoFecharForm.type = "button";
    botaoFecharForm.innerHTML = "Fechar Comentário";
    botaoFecharForm.addEventListener('click', () => {
        fecharForm(form);
    });
    notaSelect.appendChild(optNota);
    form.append(mensagem, foiAprovado, notaSelect, botaoResponderComentario, botaoFecharForm);
    document.body.append(form);
    form.addEventListener('submit', async (evt) => {
        evt.preventDefault();
        let formComentario = new FormData(form);
        formComentario.append('ProdutoID', idProduto);
        formComentario.append('UsuarioID', idUsuario);
        formComentario.append('ComentarioID', idComentario);
    let comentarioDados = {};
    for (let dado of formComentario.keys()) {
        switch (dado) {
            case 'FoiRecomendado':
                if (formComentario.get(dado) === 'on') {
                    comentarioDados[dado] = true;
                } else {
                    comentarioDados[dado] = false;
                }
                break;
            case 'Avaliacao':
                comentarioDados[dado] = parseInt(formComentario.get(dado));
                break;
            case 'ProdutoID':
                comentarioDados[dado] = parseInt(formComentario.get(dado));
                break;
            case 'ComentarioID':
                comentarioDados[dado] = parseInt(formComentario.get(dado));
                break;
            default:
                if (formComentario.get(dado) === null || formComentario.get(dado) === '') {
                    return;
                }
                comentarioDados[dado] = formComentario.get(dado);
                break;
        }
    }

    comentarioDados = JSON.stringify(comentarioDados);
    const dados = await postData('/api/comentario', comentarioDados);
    });


    


}

function fecharForm(elmt) {
    let form = document.getElementById(elmt.id);
    form.remove();
}

async function searchBarAutoComplete(query) {
    let focoAtual = -1, qtdSugestoes = 0, formPesquisar;
    query.addEventListener('input', async (evt) => {
        if (query.value.length < 2) {
            return false;
        }
        consulta = query.value;
        formPesquisar = query.parentNode.parentNode;
        closeAllLists();
        let sugestoes = await get('/api/produto/suggestions?q=' + consulta);
        qtdSugestoes = sugestoes.length;
        if (qtdSugestoes === 0) {
            return false;
        }
     
        let div1 = document.createElement('div'), div2, sugestaoAtual = '';
        div1.setAttribute("id", query.id + "autocomplete-list");
        div1.setAttribute('class', 'autocomplete-items');
        query.parentNode.appendChild(div1);
        for (let i = 0; i < qtdSugestoes; i++) {
            sugestaoAtual = sugestoes[i].nome;
            if (sugestaoAtual.substr(0, consulta.length).toUpperCase() == consulta.toUpperCase()) {
                div2 = document.createElement('div');
                div2.innerHTML = "<strong>" + sugestaoAtual.substr(0, consulta.length) + "</strong>";
                div2.innerHTML += sugestaoAtual.substr(consulta.length);
                div2.innerHTML += "<input type='hidden' value='" + sugestaoAtual + "'>";
                div2.addEventListener('click', (evt) => {
                    query.value = div2.getElementsByTagName('input')[0].value;
                    formPesquisar.submit();
                });
                div1.appendChild(div2);
            } 
        }
    });

    query.addEventListener('keydown', (evt) => {
        let x = document.getElementById(query.id + 'autocomplete-list');
        if (x) {
            x = x.getElementsByTagName('div');
        }
        if (evt.keyCode == 40) {
            focoAtual++;
            addActive(x);
        } else if (evt.keyCode == 38) {
            focoAtual--;
            addActive(x);
        } else if (evt.keyCode == 13) {
            if (focoAtual > -1) {
                if (x) {
                    x[focoAtual].click();
                }
            }
        }
    });

    function addActive(x) {
        if (!x) {
            return false;
        }

        removeActive(x);
        if (focoAtual >= X.length) {
            focoAtual = (x.length - 1)
        }

        x[focoAtual].classList.add("autocomplete-active");
    }

    function removeActive(x) {
        for (let i = 0; i < x.length; i++) {
            x[i].classList.remove('autocomplete-active');
        }
    }

    function closeAllLists(elemts) {
        var x = document.getElementsByClassName('autocomplete-items');
        for (let i = 0; i < x.length; i++) {
            if (elemts != x[i] && elemts != query) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }

    document.addEventListener('click', (evt) => {
        closeAllLists(evt.target);
    });
}

function commentsActions(element, qtdComentarios) {
    const estado = element.parentNode.style.display;
    if (estado == 'none') {
        element.parentNode.style.display = 'inherit';
        return;
    } else {
        element.parentNode.style.display = 'none';
        element.parentNode.innerHTML = 'Mostrar' + qtdComentarios + 'comentários';
        return;
    }
}

function fadeIn(elem) {
    let elemento = this;
    document.addEventListener('DOMContentLoaded', function () {
        elemento.addEventListener('focus', function () {
            elemento.setAttribute('class', 'query-focusin');
        });
    });
}
function fadeOut() {
    let elemento = this;
    return elemento.parentNode;
}