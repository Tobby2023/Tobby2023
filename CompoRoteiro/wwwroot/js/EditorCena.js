const keywords = ['Tobby', 'António', 'Da Silva', 'Maria']; // Palavras-chave para sugestões
const regExp1 = new RegExp('\\b(' + keywords.join('|') + ')(?!["<])', 'g');
let currentWord = ''; // Para armazenar a palavra atual que está sendo digitada

//var editavel = document.querySelector('div[contenteditable="true"]');
const editavel = document.getElementById('AtualizarTexto');
const informa = document.getElementById("rever");

const suggestionsList = document.getElementById('suggestionsList');

// Substituir palavras-chave no início
formatKeywords(editavel);

// Mostrar sugestões ao digitar
editavel.addEventListener('keyup', function (e) {

    const text = getCurrentWord();
    informa.innerHTML = "Currente Texto: " + text;
    if (text.length > 0) {
        currentWord = text;
        showSuggestions(text);
    } else {
        suggestionsList.style.display = 'none';
    }

    // Formatar as palavras quando o usuário digitar
    formatKeywords(this);
});

/////////=========================================================////////////////

// Monitora a remoção de conteúdo para ajustar spans vazios
editavel.addEventListener('input', function () {
    let spans = editavel.querySelectorAll('span.format');
    spans.forEach(span => {
        if (span.textContent.trim() === '') {
            span.remove(); // Remove o span se estiver vazio
        }
    });
});

function formatKeywords(element) {
    const caretPosition = saveCaretPosition(element);

    let content = element.innerHTML;

    // Remover spans existentes (para evitar duplicação)
    content = content.replace(/<a href="personagem\/([^<]+)" class="format">([^<]+)<\/a>/g, '$1');

    // Substituir as palavras-chave
    content = content.replace(regExp1, function (match) {
        return `<a href="personagem/${match}" class="format">${match}</a>`;
    });

    element.innerHTML = content;

    if (caretPosition !== null) {
        restoreCaretPosition(element, caretPosition); // Restaurar a posição do caret apenas se válido
    }
}

function manterCaretoNoFinal(element) {
    // Coloca o caret no final do conteúdo
    var range = document.createRange();
    var selection = window.getSelection();
    range.selectNodeContents(element);
    range.collapse(false);
    selection.removeAllRanges();
    selection.addRange(range);
}

function saveCaretPosition(element) {
    const selection = window.getSelection();
    if (selection.rangeCount === 0) return null; // Verifica se há uma seleção válida

    const range = selection.getRangeAt(0);
    const preCaretRange = range.cloneRange();
    preCaretRange.selectNodeContents(element);
    preCaretRange.setEnd(range.endContainer, range.endOffset);
    return preCaretRange.toString().length; // Retorna a posição atual do caret
}

function restoreCaretPosition(element, position) {
    if (position === null) return; // Verifica se a posição do caret é válida

    const selection = window.getSelection();
    const range = document.createRange();
    let charIndex = 0;
    let nodeStack = [element], node, foundStart = false, stop = false;

    while (!stop && (node = nodeStack.pop())) {
        if (node.nodeType === 3) {
            const nextCharIndex = charIndex + node.length;
            if (!foundStart && position >= charIndex && position <= nextCharIndex) {
                range.setStart(node, position - charIndex);
                range.setEnd(node, position - charIndex);
                foundStart = true;
                stop = true;
            }
            charIndex = nextCharIndex;
        } else {
            let i = node.childNodes.length;
            while (i--) {
                nodeStack.push(node.childNodes[i]);
            }
        }
    }

    selection.removeAllRanges();
    selection.addRange(range);
}



/////////=========================================================////////////////

// Obtém a palavra atual que está sendo digitada
function getCurrentWord() {
    const selection = window.getSelection();
    const range = selection.getRangeAt(0);
    const textBeforeCaret = range.startContainer.textContent.slice(0, range.startOffset);
    const match = textBeforeCaret.match(/\b(\w+)$/); // Obtém a última palavra antes do caret
    return match ? match[1] : '';
}

// Mostra a lista de sugestões
function showSuggestions(word) {
    const filteredSuggestions = keywords.filter(keyword => keyword.toLowerCase().startsWith(word.toLowerCase()));
    if (filteredSuggestions.length > 0) {
        suggestionsList.innerHTML = filteredSuggestions.map(suggestion => `<li>${suggestion}</li>`).join('');
        suggestionsList.style.display = 'block';
        positionSuggestionsBox();
    } else {
        suggestionsList.style.display = 'none';
    }
}

// Posiciona a caixa de sugestões logo abaixo do caret
function positionSuggestionsBox() {
    const range = window.getSelection().getRangeAt(0);
    const rect = range.getBoundingClientRect();
    suggestionsList.style.left = rect.left + 'px';
    suggestionsList.style.top = (rect.bottom + window.scrollY) + 'px';
}

// Substitui a palavra atual com a sugestão escolhida
suggestionsList.addEventListener('click', function (e) {
    if (e.target.tagName === 'LI') {
        insertSuggestion(e.target.textContent);
    }
});

function insertSuggestion(suggestion) {
    const selection = window.getSelection();
    const range = selection.getRangeAt(0);

    let textNode = range.startContainer;
    if (textNode.nodeType !== Node.TEXT_NODE) {
        textNode = textNode.firstChild;
    }

    const textBefore = textNode.textContent.slice(0, range.startOffset).replace(/\b\w+$/, '');
    const textAfter = textNode.textContent.slice(range.startOffset);

    informa.innerHTML = "\nCurrente Texto: " + textBefore;

    const newText = textBefore + suggestion + textAfter;
    textNode.textContent = newText;

    informa.innerHTML += "\nTexto Substituido: " + textAfter;

    // Reposiciona o caret apenas após a inserção da sugestão
    const newRange = document.createRange();
    newRange.setStart(textNode, textBefore.length + suggestion.length);
    newRange.setEnd(textNode, textBefore.length + suggestion.length);
    selection.removeAllRanges();
    selection.addRange(newRange);

    suggestionsList.style.display = 'none';
}

