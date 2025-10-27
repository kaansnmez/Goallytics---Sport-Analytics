// Fixtures page - Complete JS with Manual AJAX

if (document.readyState === 'loading') {
	document.addEventListener('DOMContentLoaded', addEventListeners);
} else {
	addEventListeners();
}

function addEventListeners() {
	// Search
	const searchInput = document.getElementById('fixtureSearch');
	const clearButton = document.getElementById('clearSearch');
	if (searchInput) searchInput.addEventListener('input', searchFixtures);
	if (clearButton) clearButton.addEventListener('click', clearSearch);

    // Prediction category toggles inside expanded prediction section
    document.addEventListener('click', function (e) {
        const tab = e.target.closest('button[data-predcat]');
        if (!tab) return;
        const card = tab.closest('.fixture-card');
        if (!card) return;
        const group = tab.getAttribute('data-predcat');
        
        // activate single-color background for selected tab
        const tabs = card.querySelectorAll('button[data-predcat]');
        tabs.forEach(b => {
            b.classList.remove('active');
            b.classList.remove('btn-success','btn-primary','btn-secondary','btn-outline-primary','btn-outline-success','btn-outline-secondary');
            b.classList.add('btn-outline-secondary');
        });
        tab.classList.add('active');
        tab.classList.remove('btn-outline-secondary');
        tab.classList.add('btn-success');
        
        // show selected group
        card.querySelectorAll('.prediction-group').forEach(el => {
            el.classList.toggle('d-none', el.getAttribute('data-group') !== group);
        });
    });

    // Indicator metric tabs toggles
    document.addEventListener('click', function (e) {
        const tab = e.target.closest('button[data-metric-tab]');
        if (!tab) return;
        const card = tab.closest('.fixture-card');
        if (!card) return;
        const key = tab.getAttribute('data-metric-tab');
        
        // styles
        card.querySelectorAll('button[data-metric-tab]').forEach(b => {
            b.classList.remove('active');
            b.classList.remove('btn-success','btn-primary','btn-secondary','btn-outline-primary','btn-outline-success','btn-outline-secondary');
            b.classList.add('btn-outline-secondary');
        });
        tab.classList.add('active');
        tab.classList.remove('btn-outline-secondary');
        tab.classList.add('btn-success');
        
        // filter groups
        card.querySelectorAll('.indicator-split').forEach(row => {
            const group = row.getAttribute('data-metric-group');
            const visible = key === 'general' ? (group === 'general') : (group === key);
            row.classList.toggle('d-none', !visible);
        });
    });

    // Filters (league, date range, status)
    const leagueFilter = document.getElementById('league-filter');
    const statusFilter = document.getElementById('status-filter');
    const btnApply = document.getElementById('apply-filters');
    if (btnApply) btnApply.addEventListener('click', handleFilter);

    // Eğer açık bölüm varsa, submit'ten önce tıklamada kapat (en garanti yol)
    document.addEventListener('click', function(e) {
        const btn = e.target && e.target.closest ? e.target.closest('form[data-ajax="true"] button[type="submit"]') : null;
        if (!btn) return;
        const form = btn.closest('form');
        const updateTarget = form && form.getAttribute ? form.getAttribute('data-ajax-update') : null;
        if (!updateTarget) return;
        const targetDiv = document.querySelector(updateTarget);
        if (!targetDiv) return;
        const isHiddenByClass = targetDiv.classList.contains('d-none');
        const cs = window.getComputedStyle(targetDiv);
        const isVisible = !isHiddenByClass && cs.display !== 'none' && cs.visibility !== 'hidden' && cs.opacity !== '0';
        if (isVisible) {
            // Toggle: açık ise kapat ve submit'i engelle
            targetDiv.classList.add('d-none');
            targetDiv.style.display = 'none';
            setLoading(btn, false);
            e.preventDefault();
            if (typeof e.stopImmediatePropagation === 'function') e.stopImmediatePropagation();
            return false;
        }
    }, true);

    // Setup MANUAL AJAX handlers
    setupManualAjaxHandlers();
}

function handleFilter() {
    const league = document.getElementById('league-filter')?.value || 'all';
    const status = document.getElementById('status-filter')?.value || 'all';
    const from = document.getElementById('date-from')?.value;

    const items = document.querySelectorAll('#fixtures-list > .col-12');
    items.forEach(item => {
        const card = item.querySelector('.fixture-card');
        if (!card) { item.style.display = ''; return; }

        const text = item.innerText.toLowerCase();
        let ok = true;

        // status match by badge/text
        if (status !== 'all') {
            ok = text.includes(status.toLowerCase());
        }

        // league match by badge text
        if (ok && league !== 'all') {
            ok = text.includes(league.replace('-', ' '));
        }

        // date range using date in card (try to parse YYYY-MM-DD or dd.MM.yyyy)
        if (ok && from) {
            const dateMatch = item.querySelector('.date-utc');
            let d = null;
            if (dateMatch) {
                const val = dateMatch.textContent.trim();
                // try parse
                const iso = val.replace(' ', 'T');
                d = new Date(iso);
                if (isNaN(d.getTime())) {
                    // try dd.MM.yyyy
                    const parts = val.split(/[ .:]/);
                    if (parts.length >= 3) {
                        const ddmmyy = `${parts[0].padStart(2,'0')}-${parts[1].padStart(2,'0')}-${parts[2]}`;
                        d = new Date(ddmmyy);
                    }
                }
            }
            if (d) {
                const df = new Date(from + 'T00:00:00');
                const dt = new Date(from + 'T23:59:59');
                ok = d >= df && d <= dt;
            }
        }

        item.style.display = ok ? '' : 'none';
    });
}

function searchFixtures() {
	const term = (document.getElementById('fixtureSearch')?.value || '').toLowerCase().trim();
	const rows = document.querySelectorAll('#fixtures-list > .col-12');
	rows.forEach(row => {
		const txt = row.innerText.toLowerCase();
		row.style.display = txt.includes(term) ? '' : 'none';
	});
}

function clearSearch() {
	const input = document.getElementById('fixtureSearch');
	if (input) input.value = '';
	const rows = document.querySelectorAll('#fixtures-list > .col-12');
	rows.forEach(row => row.style.display = '');
}

// ========================================
// MANUAL AJAX Handlers
// ========================================

function setupManualAjaxHandlers() {
    console.log('✅ Manual AJAX handlers yüklendi');

    // Tüm AJAX form'ları dinle
    document.addEventListener('submit', function(e) {
        const form = e.target;
        
        // Sadece data-ajax="true" olan formları işle
        if (!form || form.getAttribute('data-ajax') !== 'true') {
            return;
        }

        e.preventDefault();
        e.stopPropagation();

        const button = form.querySelector('button[type="submit"]');
        if (!button) return;

        const action = button.getAttribute('data-action');
        const updateTarget = form.getAttribute('data-ajax-update');
        const ajaxUrl = form.getAttribute('action');
        
        console.log('🔄 AJAX başlatılıyor:', {
            action: action,
            target: updateTarget,
            url: ajaxUrl
        });

        if (!updateTarget || !ajaxUrl) {
            console.error('❌ data-ajax-update veya action eksik!');
            return;
        }

        const targetDiv = document.querySelector(updateTarget);
        if (!targetDiv) {
            console.error('❌ Target div bulunamadı:', updateTarget);
            return;
        }

        const card = targetDiv.closest('.fixture-card');
        if (!card) {
            console.error('❌ Fixture card bulunamadı');
            return;
        }

        // Eğer target div zaten açıksa, kapat (toggle)
        const isHiddenByClass = targetDiv.classList.contains('d-none');
        const cs = window.getComputedStyle(targetDiv);
        const isVisible = !isHiddenByClass && cs.display !== 'none' && cs.visibility !== 'hidden' && cs.opacity !== '0';
        if (isVisible) {
            console.log('🔽 Div zaten açık, kapatılıyor');
            targetDiv.classList.add('d-none');
            targetDiv.style.display = 'none';
            // Her ihtimale karşı butonu da eski haline getir
            setLoading(button, false);
            // Form submit'ini engelle
            e.preventDefault();
            if (typeof e.stopImmediatePropagation === 'function') e.stopImmediatePropagation();
            return false;
        }

        // Diğer tüm expanded div'leri kapat
        card.querySelectorAll('.match-details-expanded, .match-prediction-expanded').forEach(div => {
            div.classList.add('d-none');
            div.style.display = 'none';
        });

        // Birden fazla tıklamayı engelle ve loading göster
        if (button.dataset.loading === '1') {
            return;
        }
        const originalHtml = button.innerHTML;
        button.dataset.originalHtml = originalHtml;
        setLoading(button, true);

        // Form verilerini topla
        const formData = new FormData(form);

        // AJAX isteği gönder
        fetch(ajaxUrl, {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
        .then(response => {
            console.log('📥 Response alındı:', response.status);
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            return response.text();
        })
        .then(html => {
            console.log('✅ HTML alındı, uzunluk:', html.length);
            
            if (!html || html.trim().length === 0) {
                throw new Error('Boş response');
            }

            // HTML'i target div'e yerleştir
            targetDiv.innerHTML = html;
            
            // Div'i göster
            targetDiv.classList.remove('d-none');
            targetDiv.style.display = 'block';
            
            console.log('👁️ Div gösterildi:', updateTarget);
        })
        .catch(error => {
            console.error('❌ AJAX hatası:', error);
            
            // Hata mesajını göster
            targetDiv.innerHTML = `
                <div class="alert alert-danger m-3">
                    <i class="fas fa-exclamation-circle me-2"></i>
                    ${error.message || 'Veri yüklenirken hata oluştu'}
                </div>
            `;
            targetDiv.classList.remove('d-none');
            targetDiv.style.display = 'block';
            
            showNotification(error.message || 'Veri yüklenirken hata oluştu', 'danger');
        })
        .finally(() => {
            setLoading(button, false);
        });
    }, true); // true = capture phase (önce bu handler çalışır)
}

function setLoading(button, isLoading) {
    try {
        if (!button) return;
        if (isLoading) {
            button.dataset.loading = '1';
            button.disabled = true;
            const html = button.dataset.originalHtml || button.innerHTML;
            button.dataset.originalHtml = html;
            button.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span>Yükleniyor...';
        } else {
            button.disabled = false;
            button.dataset.loading = '0';
            const original = button.dataset.originalHtml;
            if (original) {
                button.innerHTML = original;
            }
        }
    } catch (_) { /* no-op */ }
}

// Bildirim göster (Toast benzeri)
function showNotification(message, type = 'info') {
    // Bootstrap toast varsa kullan
    if (typeof bootstrap !== 'undefined' && bootstrap.Toast) {
        // Toast container yoksa oluştur
        let toastContainer = document.getElementById('toast-container');
        if (!toastContainer) {
            toastContainer = document.createElement('div');
            toastContainer.id = 'toast-container';
            toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
            toastContainer.style.zIndex = '9999';
            document.body.appendChild(toastContainer);
        }
        
        // Toast oluştur
        const toastId = 'toast-' + Date.now();
        const toastHtml = `
            <div id="${toastId}" class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        `;
        
        toastContainer.insertAdjacentHTML('beforeend', toastHtml);
        
        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement, { delay: 3000 });
        toast.show();
        
        // Toast kapandıktan sonra DOM'dan kaldır
        toastElement.addEventListener('hidden.bs.toast', function() {
            toastElement.remove();
        });
    } else {
        // Fallback: Console
        console.log(message);
    }
}

// Prediction group göster (inline onclick için)
function showPredictionGroup(btn, groupName) {
    try {
        // Tüm tab butonlarından active sınıfını kaldır
        const parentGroup = btn.closest('.prediction-tabs');
        if (!parentGroup) return;
        
        parentGroup.querySelectorAll('button').forEach(b => {
            b.classList.remove('active', 'btn-success', 'btn-primary', 'btn-secondary');
            
            // Outline stilini geri yükle
            const originalColor = groupName === 'match' ? 'success' : 
                                 groupName === 'btts' ? 'secondary' : 'primary';
            b.classList.add('btn-outline-' + originalColor);
        });
        
        // Tıklanan butonu aktif yap
        btn.classList.remove('btn-outline-success', 'btn-outline-primary', 'btn-outline-secondary');
        btn.classList.add('active', 'btn-success');
        
        // Tüm prediction gruplarını gizle
        const container = btn.closest('.details-content');
        if (!container) return;
        
        container.querySelectorAll('.prediction-group').forEach(group => {
            group.classList.add('d-none');
        });
        
        // Seçilen grubu göster
        const targetGroup = container.querySelector(`.prediction-group[data-group="${groupName}"]`);
        if (targetGroup) {
            targetGroup.classList.remove('d-none');
        }
        
        console.log('📊 Prediction group değiştirildi:', groupName);
    } catch (err) {
        console.error('Prediction group gösterme hatası:', err);
    }
}

// Global scope'a export et
if (typeof window !== 'undefined') {
    window.showPredictionGroup = showPredictionGroup;
    window.showNotification = showNotification;
    // Fallback: Inline onclick'ler için güvenli tetikleyici (geçiş süreci)
    window.toggleSectionByButton = function(buttonEl, sectionType) {
        try {
            const form = buttonEl && buttonEl.closest ? buttonEl.closest('form') : null;
            if (!form) return false;
            // Mevcut açık bölümü kapatma davranışını koru: submit handler halledecek
            const evt = new Event('submit', { bubbles: true, cancelable: true });
            form.dispatchEvent(evt);
            return false; // inline onclick default'ını iptal et
        } catch (_) {
            return false;
        }
    };
}
if (typeof globalThis !== 'undefined') {
    globalThis.showPredictionGroup = showPredictionGroup;
    globalThis.showNotification = showNotification;
    globalThis.toggleSectionByButton = function(buttonEl, sectionType) {
        try {
            const form = buttonEl && buttonEl.closest ? buttonEl.closest('form') : null;
            if (!form) return false;
            const evt = new Event('submit', { bubbles: true, cancelable: true });
            form.dispatchEvent(evt);
            return false;
        } catch (_) {
            return false;
        }
    };
}

console.log('📈 Fixtures.js loaded and ready');
