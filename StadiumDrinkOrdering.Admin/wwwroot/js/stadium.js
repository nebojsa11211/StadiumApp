// Stadium structure management JavaScript helpers

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    
    URL.revokeObjectURL(url);
};

window.hoverTribune = (tribuneCode) => {
    try {
        const tribuneElement = document.querySelector(`[data-tribune="${tribuneCode}"]`);
        if (tribuneElement) {
            tribuneElement.style.opacity = '0.8';
            tribuneElement.style.transform = 'scale(1.05)';
            tribuneElement.style.transition = 'all 0.2s ease';
        }
    } catch (error) {
        console.warn('Error hovering tribune:', error);
    }
};

window.unhoverTribune = () => {
    try {
        const tribuneElements = document.querySelectorAll('[data-tribune]');
        tribuneElements.forEach(element => {
            element.style.opacity = '';
            element.style.transform = '';
            element.style.transition = '';
        });
    } catch (error) {
        console.warn('Error unhovering tribunes:', error);
    }
};