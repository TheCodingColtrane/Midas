async function getCep(cep) {
    let novoCep = String(cep);
    novoCep = novoCep.replace('-', '');
    await fetch('/api/endereco/' + novoCep).then(function (res) {
        if (res.ok) {
            const dados = res.json();
            if (dados != {}) {
                return dados;
            }
        }
    }).catch(function (err) {
        alert(err);
    });

    await fetch('https://viacep.com.br/ws/' + novoCep + '/json').then(function (res) {
        res.json().then(function (dados) {
            return dados;
        }).catch(function (err) {
            return err;
        });
    });
}
/**
 *Faz uma requisição GET ao URI designado.
 * @param {any} uri
 */
async function get(uri) {
    try {
        const controller = new AbortController();
        const signalController = controller.signal;
        let headers = new Headers();
        headers.append('accept', 'application/json');
        headers.append('content-type', 'application/json');
        headers.append('expires', new Date().setMinutes(1, 30));
        const getConfig = {
            method: 'GET',
            signal: signalController,
            headers: headers
        };


        const req = await fetch(uri, getConfig);
        if (signalController.aborted) {
            alert('Houve um erro');
        }
        return await req.json();
    }
    catch (err) {
        alert(err);
    }

}
/**
 * Faz uma requisição POST ao URI designado. 
 * @param {string} uri
 * @param {any} data
 */
async function postData(uri, data) {
    try {
        let cabecalhos = new Headers();
        cabecalhos.append('content-type', 'application/json');
        cabecalhos.append('accept', 'application/json');
        cabecalhos.append('expires', new Date().setHours(0, 1, 30));
        const controller = new AbortController();
        const controllerSignal = controller.signal;
        const postDataConfig = {
            method: 'POST',
            headers: cabecalhos,
            body: data,
            signal: controllerSignal
        };
        let req = await fetch(uri, postDataConfig);
        let dados = await req.json();
        if (req.ok) {
            return dados;
        }
    }

    catch (err) {
        new Error('Não foi possível publicar seu comentário', err);
    }

}
const urlSlug = (recurso) => recurso.toLowerCase().replace(/[^\w ]+/g, '').replace(/ +/g, '-');

/**
 * Retorna o valor de um cookie, se este existir.
 * @param {String} cookieName
 */
function getCookie(cookieName) {
    let name = cookieName + '=';
    const cookieDecoficado = decodeURIComponent(document.cookie);
    let ca = cookieDecoficado.split(';');
    let caracterCookie = '';
    for (let i = 0; i < ca.length; i++) {
        caracterCookie = ca[i];
        while (caracterCookie.charAt(0) == ' ') {
            caracterCookie = caracterCookie.substring(1);
        }

        if (caracterCookie.indexOf(name) == 0) {
            return caracterCookie.substring(name.length, caracterCookie.length);
        }
    }

    return '';
}

function getCartCookieData(cookieName) {
    let name = cookieName + '=';
    const cookieDecoficado = decodeURI(document.cookie);
    let ca = cookieDecoficado.split(';');
    let caracterCookie = '';
    for (let i = 0; i < ca.length; i++) {
        caracterCookie = ca[i];
        while (caracterCookie.charAt(0) == ' ') {
            caracterCookie = caracterCookie.substring(1);
        }

        if (caracterCookie.indexOf(name) == 0) {
            caracterCookie = caracterCookie.substring(name.length, caracterCookie.length);
        }

        let data = {
            productId: [],
            productPrice: [],
            productName: [],
            productURL: [],
            productQuantity: []
        };

        if (caracterCookie === '') {
            return data;
        } else {
            let cookieData = caracterCookie;
            //cookieData = cookieData.replace(/\[/g, '').replace(/\]/g, '').replace(/['"]+/g,'').replace(/[\\]+/g,'');
            cookieData = cookieData.split('|');
            cookieData = cookieData.filter(function (val) {
                return val != "";
            });
            //cookieData = cookieData.split(',').map(function (data) {
            //    return data.replace(/\""""/g, '');
            //});

            let linhas = cookieData.length / 5, aux = 0;
            for (let linha = 0; linha < linhas; linha++) {
                aux += 5;
                data.productId[linha] = parseInt(cookieData[aux - 5]);
                data.productName[linha] = cookieData[aux - 4];
                data.productURL[linha] = cookieData[aux - 3];
                data.productPrice[linha] = parseFloat(cookieData[aux - 2]);
                data.productQuantity[linha] = parseInt(cookieData[aux - 1]);

            }

            return data;



        }
    }
}


function editCookieData(cookieName, productName, productQuantity) {
    let cookieData = getCartCookieData(cookieName);
    const index = cookieData.productName.findIndex(value => value === productName);
    cookieData.productQuantity[index] = parseInt(productQuantity);
    cookieData.productPrice[index] = parseFloat(cookieData.productPrice[index] * productQuantity);
    return setCookie(cookieName, 10, cookieData, 1);

}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}

function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
}
/**
 * Cria um cookie. 
 * @param {String} cookieName
 * @param {Number} daysToExpire
 * @param {any} cookieData
 */
function setCookie(cookieName, daysToExpire, cookieData, tipo = 0) {
    let existingCookieSavedData = [], qtdProdutosNoCarrinho = 0, cartProducts = '';
    if (tipo === 0) {
        existingCookieSavedData = getCookie(cookieName);
    }
    let cookieExpireDate = new Date();
    cookieExpireDate.setDate(cookieExpireDate.getDate() + daysToExpire);
    if (existingCookieSavedData === '') {
        document.cookie = cookieName + '=' + encodeURI(cookieData) + ';Expires=' + cookieExpireDate.toUTCString() + ';Path=/;Secure=true';
        return;
    } else {
        if (tipo === 1) {
            let indice = -1, repeats = 0;
            qtdProdutosNoCarrinho = cookieData.productId.length;
            repeats = cookieData.productId.filter(value => value == cookieData.productId[cookieData.productId.length - 1]).length;
            if (qtdProdutosNoCarrinho > 1) {
                for (let i = 0; i < cookieData.productId.length; i++) {
                    if (repeats > 1) {
                        break;
                    }

                    cartProducts += '|' + parseInt(cookieData.productId[i]) + '|' + cookieData.productName[i] + '|'
                        + cookieData.productURL[i] + '|' + parseFloat(cookieData.productPrice[i]) + '|' + parseInt(cookieData.productQuantity[i]);
                }
             
            } else {
                indice = -1;
                cartProducts = '|' + parseInt(cookieData.productId) + '|' + cookieData.productName + '|'
                    + cookieData.productURL + '|' + parseFloat(cookieData.productPrice) + '|' + parseInt(cookieData.productQuantity);
            }

            if (indice != -1) {
                modalMensagem('Este produto já está no carrinho.');
                return false;
            } else {
                document.cookie = cookieName + '=' + encodeURI(cartProducts) + ';Expires=' + cookieExpireDate.toUTCString() + ';Path=/;Secure=true';
                document.querySelector('#productAmount').value = cartProductUpdate();
                return true;
            }
        }

    }

}
/**
 * Apresenta uma imagem personalizada ao usuário, em nível de severidade.
 * @param {String} msg
 * @param {Number} tipo
 */
function modalMensagem(msg, tipo = 0) {
    let divModalMsg = document.createElement('div');
    let divModalMsgContent = document.createElement('div');
    divModalMsg.className = 'modal-msg';
    divModalMsgContent.className = 'modal-msg-content';
    divModalMsg.appendChild(divModalMsgContent);
    let paragrafo = document.createElement('p');
    paragrafo.innerHTML = msg;
    divModalMsgContent.appendChild(paragrafo);
    divModalMsg.style.display = 'block';
    document.body.appendChild(divModalMsg);
    setTimeout(() => {
        document.body.removeChild(divModalMsg);
    }, 5000);
}
/**
 * Remove um item do carrinho, se este existir.
 * @param {Number} id
 */
function removeCartProduct(id) {
    id = parseInt(id);
    let cookieData = getCartCookieData('mdc');
    const index = cookieData.productId.findIndex(value => value === id);
    if (index === -1) {
        modalMensagem('Não foi possível remover o item do carrinho.');
        return false;
    } else {
        cookieData.productId.splice(index, 1);
        cookieData.productName.splice(index, 1);
        cookieData.productURL.splice(index, 1);
        cookieData.productPrice.splice(index, 1);
        cookieData.productQuantity.splice(index, 1);
        const isRemoved = setCookie('mdc', 10, cookieData, 1);
        if (isRemoved) {
            document.body.removeChild(document.querySelector('#s_product_' + id));
            modalMensagem('Produto removido do carrinho com sucesso.');
            document.querySelector('#productAmount').value = cartProductUpdate(); 
        }
        
    }
}

/**
 * Atualiza a quantidade de produtos presentes no carrinho ao DOM.
 */
function cartProductUpdate() {
    const cartProductsAmount = getCartCookieData().productId.length;
    if (cartProductsAmount > 0) {
        return cartProductsAmount;
    } else {
        return 0;
    }
} 
