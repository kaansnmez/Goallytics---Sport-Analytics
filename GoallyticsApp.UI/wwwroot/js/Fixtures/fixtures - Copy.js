// Fixtures page - Complete JS with AJAX support

if (document.readyState === 'loading') {
	document.addEventListener('DOMContentLoaded', addEventListeners);
} else {
	addEventListeners();
}

function addEventListeners() {
	// Toggle expandable sections on buttons inside static cards
    document.addEventListener('click', function (e) {
        const btn = e.target.closest('button[data-action]');
		if (!btn) return;
        // If button already has inline onclick, let it handle toggle to avoid double-toggle
        if (btn.getAttribute && btn.getAttribute('onclick')) return;
		const card = btn.closest('.fixture-card');
		if (!card) return;
		const action = btn.getAttribute('data-action');
		const details = card.querySelector('.match-details-expanded');
		const prediction = card.querySelector('.match-prediction-expanded');

		if (action === 'details' && details) {
            // toggle details with Bootstrap's d-none fallback
            const isHidden = details.classList.contains('d-none') || details.style.display === 'none';
            details.classList.toggle('d-none', !isHidden);
            details.style.display = isHidden ? 'block' : 'none';
            if (prediction) {
                prediction.classList.add('d-none');
                prediction.style.display = 'none';
            }
		} else if (action === 'prediction' && prediction) {
            const isHidden = prediction.classList.contains('d-none') || prediction.style.display === 'none';
            prediction.classList.toggle('d-none', !isHidden);
            prediction.style.display = isHidden ? 'block' : 'none';
            if (details) {
                details.classList.add('d-none');
                details.style.display = 'none';
            }
		}
	});

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

    // Setup AJAX handlers if jQuery is available
    if (typeof $ !== 'undefined') {
        setupAjaxHandlers();
    }
}

// Fallback explicit toggler usable from HTML onclick
function toggleSectionByButton(buttonEl, sectionType) {
    try {
        const card = buttonEl.closest('.fixture-card');
        if (!card) return;
        const details = card.querySelector('.match-details-expanded');
        const prediction = card.querySelector('.match-prediction-expanded');
        if (sectionType === 'details' && details) {
            const hidden = details.classList.contains('d-none') || details.style.display === 'none' || !details.style.display;
            details.classList.toggle('d-none', !hidden);
            details.style.display = hidden ? 'block' : 'none';
            if (prediction) { prediction.classList.add('d-none'); prediction.style.display = 'none'; }
        } else if (sectionType === 'prediction' && prediction) {
            const hidden = prediction.classList.contains('d-none') || prediction.style.display === 'none' || !prediction.style.display;
            prediction.classList.toggle('d-none', !hidden);
            prediction.style.display = hidden ? 'block' : 'none';
            if (details) { details.classList.add('d-none'); details.style.display = 'none'; }
        }
    } catch (_) { /* no-op */ }
}

// Expose for inline onclick handlers in server-rendered HTML
if (typeof window !== 'undefined') {
    window.toggleSectionByButton = toggleSectionByButton;
}
if (typeof globalThis !== 'undefined') {
    globalThis.toggleSectionByButton = toggleSectionByButton;
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

// Optional helpers if needed later
function getStatusClass(status) {
	switch ((status || '').toString().toLowerCase()) {
		case 'ft':
		case 'finished':
			return 'status-finished';
		case 'live':
		case '1h':
		case '2h':
		case 'ht':
			return 'status-live';
		case 'scheduled':
		case 'ns':
			return 'status-scheduled';
		case 'postponed':
		case 'cancelled':
			return 'status-cancelled';
		default:
			return 'status-default';
	}
}

function getStatusText(status) {
	switch ((status || '').toString().toLowerCase()) {
		case 'scheduled':
		case 'ns':
			return 'Planlanan';
		case 'live':
		case '1h':
		case '2h':
		case 'ht':
			return 'Canlı';
		case 'ft':
		case 'finished':
			return 'Biten';
		default:
			return 'Bilinmiyor';
	}
}

// ========================================
// AJAX Handlers for Predictions & Details
// ========================================

function setupAjaxHandlers() {
    console.log('✅ AJAX handlers yüklendi');

    // AJAX form submit başlamadan önce
    $(document).on('submit', 'form[data-ajax="true"]', function(e) {
        const form = $(this);
        const button = form.find('button[type="submit"]');
        const action = button.data('action');
        
        console.log('🔄 AJAX başlatılıyor:', action);
        
        // Butonu disable et ve loading göster
        button.prop('disabled', true);
        
        const originalHtml = button.html();
        button.data('original-html', originalHtml);
        
        button.html('<span class="spinner-border spinner-border-sm me-1" role="status"></span>Yükleniyor...');
    });

    // AJAX başarılı olduğunda
    $(document).on('ajaxSuccess', 'form[data-ajax="true"]', function(event, xhr, settings) {
        const form = $(this);
        const button = form.find('button[type="submit"]');
        const action = button.data('action');
        
        console.log('✅ AJAX başarılı:', action);
        console.log('Response uzunluğu:', xhr.responseText ? xhr.responseText.length : 0);
        
        // Butonu normale döndür
        restoreButton(button);
        
        // Response boşsa uyarı göster
        if (!xhr.responseText || xhr.responseText.trim().length === 0) {
            console.warn('⚠️ Boş response alındı');
            showNotification('Veri bulunamadı', 'warning');
        } else {
            console.log('📦 Veri yüklendi');
            
            // Eğer prediction ise, ilgili div'i göster
            if (action === 'prediction') {
                const updateTarget = form.data('ajax-update');
                if (updateTarget) {
                    const targetDiv = $(updateTarget);
                    if (targetDiv.length > 0) {
                        targetDiv.removeClass('d-none');
                        targetDiv.show();
                        console.log('👁️ Prediction div gösterildi:', updateTarget);
                        
                        // Diğer div'i gizle (details)
                        const card = targetDiv.closest('.fixture-card');
                        const detailsDiv = card.find('.match-details-expanded');
                        if (detailsDiv.length > 0) {
                            detailsDiv.addClass('d-none');
                            detailsDiv.hide();
                        }
                    }
                }
            } else if (action === 'details') {
                const updateTarget = form.data('ajax-update');
                if (updateTarget) {
                    const targetDiv = $(updateTarget);
                    if (targetDiv.length > 0) {
                        targetDiv.removeClass('d-none');
                        targetDiv.show();
                        console.log('👁️ Details div gösterildi:', updateTarget);
                        
                        // Diğer div'i gizle (prediction)
                        const card = targetDiv.closest('.fixture-card');
                        const predictionDiv = card.find('.match-prediction-expanded');
                        if (predictionDiv.length > 0) {
                            predictionDiv.addClass('d-none');
                            predictionDiv.hide();
                        }
                    }
                }
            }
        }
    });

    // AJAX hata durumunda
    $(document).on('ajaxError', 'form[data-ajax="true"]', function(event, xhr, settings, error) {
        const form = $(this);
        const button = form.find('button[type="submit"]');
        const action = button.data('action');
        
        console.error('❌ AJAX hatası:', action);
        console.error('Status:', xhr.status);
        console.error('Error:', error);
        console.error('Response:', xhr.responseText);
        
        // Butonu normale döndür
        restoreButton(button);
        
        // Hata mesajı göster
        let errorMessage = 'Veri yüklenirken hata oluştu';
        if (xhr.status === 404) {
            errorMessage = 'Veri bulunamadı';
        } else if (xhr.status === 500) {
            errorMessage = 'Sunucu hatası oluştu';
        } else if (xhr.status === 0) {
            errorMessage = 'Bağlantı hatası';
        }
        
        showNotification(errorMessage, 'danger');
    });

    // AJAX tamamlandığında (başarılı veya hatalı)
    $(document).on('ajaxComplete', 'form[data-ajax="true"]', function() {
        const form = $(this);
        const button = form.find('button[type="submit"]');
        
        console.log('🏁 AJAX tamamlandı');
        
        // Eğer buton hala disabled ise, restore et
        if (button.prop('disabled')) {
            restoreButton(button);
        }
    });
}

// Butonu orijinal haline döndür
function restoreButton(button) {
    button.prop('disabled', false);
    
    const originalHtml = button.data('original-html');
    if (originalHtml) {
        button.html(originalHtml);
    } else {
        // Fallback
        const action = button.data('action');
        if (action === 'prediction') {
            button.html('Tahmini Gör');
        } else if (action === 'details') {
            button.html('<i class="fas fa-chart-bar me-1"></i>Detaylı İstatistik');
        }
    }
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
        // Fallback: Alert
        alert(message);
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
}
if (typeof globalThis !== 'undefined') {
    globalThis.showPredictionGroup = showPredictionGroup;
    globalThis.showNotification = showNotification;
}
