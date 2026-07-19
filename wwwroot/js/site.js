// DrinkStore - small UX helpers
document.addEventListener('DOMContentLoaded', function () {
  // Quantity stepper buttons (used on product details & cart pages)
  document.querySelectorAll('[data-qty-step]').forEach(function (btn) {
    btn.addEventListener('click', function () {
      var targetId = btn.getAttribute('data-target');
      var input = document.getElementById(targetId);
      if (!input) return;
      var step = parseInt(btn.getAttribute('data-qty-step'), 10);
      var min = parseInt(input.getAttribute('min') || '1', 10);
      var value = parseInt(input.value || '1', 10) + step;
      if (value < min) value = min;
      input.value = value;
    });
  });

  // Auto-dismiss alerts after 4s
  document.querySelectorAll('.alert').forEach(function (el) {
    setTimeout(function () {
      var alert = bootstrap.Alert.getOrCreateInstance(el);
      alert.close();
    }, 4000);
  });
});
