window.getAllInputs = () => {
    return Array.from(document.querySelectorAll("input")).map(input => ({
        id: input.id || null,
        name: input.name || null,
        type: input.type,
        value: input.value
    }));
};