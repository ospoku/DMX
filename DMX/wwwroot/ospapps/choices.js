document.addEventListener('DOMContentLoaded', () =>
{
    const selects = document.querySelectorAll('select.dmx-choices');
    selects.forEach(select => {
        const choices = new Choices(select,
            {
                removeItemButton: true,
                searchEnabled: true,
                itemSelectText: '',
                shouldSort: false,
                placeholder: true,
               /* placeholderValue: 'Select Approver(s)'*/
            });
    });
});    