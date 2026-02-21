using HardWareMonitor.Models;
using HardWareMonitor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace HardWareMonitor.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private CpuInfo _cpuInfo;
        public CpuInfo CpuInfo
        {
            get { return _cpuInfo; }
            set { SetProperty(ref _cpuInfo, value); }
        }

        private MemoryInfo _memoryInfo;
        public MemoryInfo MemoryInfo
        {
            get { return _memoryInfo; }
            set { SetProperty(ref _memoryInfo, value); }
        }

        private DiskInfo _diskInfo;
        public DiskInfo DiskInfo
        {
            get { return _diskInfo; }
            set { SetProperty(ref _diskInfo, value); }
        }

        private GpuInfo _gpuInfo;
        public GpuInfo GpuInfo
        {
            get { return _gpuInfo; }
            set { SetProperty(ref _gpuInfo, value); }
        }

        private NetworkAdapterInfo _networkInfo;
        public NetworkAdapterInfo NetworkInfo
        {
            get { return _networkInfo; }
            set { SetProperty(ref _networkInfo, value); }
        }

        private SystemInfo _systemInfo;
        public SystemInfo SystemInfo
        {
            get { return _systemInfo; }
            set { SetProperty(ref _systemInfo, value); }
        }

        private Visibility _isCpuVisible = Visibility.Visible;
        public Visibility IsCpuVisible
        {
            get { return _isCpuVisible; }
            set { SetProperty(ref _isCpuVisible, value); }
        }

        private Visibility _isMemoryVisible = Visibility.Collapsed;
        public Visibility IsMemoryVisible
        {
            get { return _isMemoryVisible; }
            set { SetProperty(ref _isMemoryVisible, value); }
        }

        private Visibility _isDiskVisible = Visibility.Collapsed;
        public Visibility IsDiskVisible
        {
            get { return _isDiskVisible; }
            set { SetProperty(ref _isDiskVisible, value); }
        }

        private Visibility _isGpuVisible = Visibility.Collapsed;
        public Visibility IsGpuVisible
        {
            get { return _isGpuVisible; }
            set { SetProperty(ref _isGpuVisible, value); }
        }

        private Visibility _isNetworkVisible = Visibility.Collapsed;
        public Visibility IsNetworkVisible
        {
            get { return _isNetworkVisible; }
            set { SetProperty(ref _isNetworkVisible, value); }
        }

        private Visibility _isTaskManagerVisible = Visibility.Collapsed;
        public Visibility IsTaskManagerVisible
        {
            get { return _isTaskManagerVisible; }
            set { SetProperty(ref _isTaskManagerVisible, value); }
        }

        private Visibility _isSystemInfoVisible = Visibility.Visible;
        public Visibility IsSystemInfoVisible
        {
            get { return _isSystemInfoVisible; }
            set { SetProperty(ref _isSystemInfoVisible, value); }
        }

        private string _statusMessage = "Готов к работе";
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        private int _updateInterval = 3;
        public int UpdateInterval
        {
            get { return _updateInterval; }
            set { SetProperty(ref _updateInterval, value); }
        }

        private string _updateButtonText = "Стоп";
        public string UpdateButtonText
        {
            get { return _updateButtonText; }
            set { SetProperty(ref _updateButtonText, value); }
        }

        private string _updateButtonColor = "#F44336";
        public string UpdateButtonColor
        {
            get { return _updateButtonColor; }
            set { SetProperty(ref _updateButtonColor, value); }
        }

        private string _updateStatusText = "Обновление каждые 3 сек";
        public string UpdateStatusText
        {
            get { return _updateStatusText; }
            set { SetProperty(ref _updateStatusText, value); }
        }

        private bool _isUpdating = true;

        private string _taskManagerSearchText = "";
        public string TaskManagerSearchText
        {
            get { return _taskManagerSearchText; }
            set
            {
                SetProperty(ref _taskManagerSearchText, value);
                OnPropertyChanged(nameof(FilteredProcesses));
                OnPropertyChanged(nameof(FilteredProcessesCount));
            }
        }

        private List<ProcessInfo> _allProcesses = new List<ProcessInfo>();
        public List<ProcessInfo> FilteredProcesses
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TaskManagerSearchText))
                    return _allProcesses;

                return _allProcesses
                    .Where(p => p.Name.IndexOf(TaskManagerSearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
        }

        public int FilteredProcessesCount => FilteredProcesses.Count;
        public int TotalProcessesCount => _allProcesses.Count;

        public long TotalMemoryUsage => _allProcesses.Sum(p => p.WorkingSet);
        public double TotalMemoryUsageMb => TotalMemoryUsage / (1024.0 * 1024.0);

        public ICommand RefreshCommand { get; }
        public ICommand ShowCpuCommand { get; }
        public ICommand ShowMemoryCommand { get; }
        public ICommand ShowDiskCommand { get; }
        public ICommand ShowGpuCommand { get; }
        public ICommand ShowNetworkCommand { get; }
        public ICommand ShowTaskManagerCommand { get; }
        public ICommand ShowHelpCommand { get; }
        public ICommand ApplyIntervalCommand { get; }
        public ICommand ToggleUpdateCommand { get; }
        public ICommand ExportTxtCommand { get; }
        public ICommand ExportCsvCommand { get; }
        public ICommand ExportJsonCommand { get; }
        public ICommand RefreshTaskManagerCommand { get; }

        private System.Timers.Timer _autoRefreshTimer;
        private readonly CpuMonitor _cpuMonitor = new CpuMonitor();
        private readonly MemoryMonitor _memoryMonitor = new MemoryMonitor();
        private readonly DiskMonitor _diskMonitor = new DiskMonitor();
        private readonly GpuMonitor _gpuMonitor = new GpuMonitor();
        private readonly NetworkMonitor _networkMonitor = new NetworkMonitor();
        private readonly SystemMonitor _systemMonitor = new SystemMonitor();
        private readonly ProcessMonitor _processMonitor = new ProcessMonitor();

        // Инициализирует команды и запускает автообновление
        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(async _ => await RefreshAllDataAsync());
            ShowCpuCommand = new RelayCommand(_ => ShowCpuSection());
            ShowMemoryCommand = new RelayCommand(_ => ShowMemorySection());
            ShowDiskCommand = new RelayCommand(_ => ShowDiskSection());
            ShowGpuCommand = new RelayCommand(_ => ShowGpuSection());
            ShowNetworkCommand = new RelayCommand(_ => ShowNetworkSection());
            ShowTaskManagerCommand = new RelayCommand(_ => ShowTaskManagerSection());
            ShowHelpCommand = new RelayCommand(_ => ShowHelp());
            ApplyIntervalCommand = new RelayCommand(_ => ApplyInterval());
            ToggleUpdateCommand = new RelayCommand(_ => ToggleUpdate());
            ExportTxtCommand = new RelayCommand(_ => ExportTxt());
            ExportCsvCommand = new RelayCommand(_ => ExportCsv());
            ExportJsonCommand = new RelayCommand(_ => ExportJson());
            RefreshTaskManagerCommand = new RelayCommand(_ => RefreshTaskManager());

            StartAutoRefresh();

            Task.Run(async () => await RefreshAllDataAsync());
        }

        // Запускает таймер автоматического обновления данных
        private void StartAutoRefresh()
        {
            _autoRefreshTimer = new System.Timers.Timer(3000);
            _autoRefreshTimer.Elapsed += async (s, e) => await RefreshAllDataAsync();
            _autoRefreshTimer.AutoReset = true;
            _autoRefreshTimer.Start();
        }

        // Обновляет все данные асинхронно: информацию о компонентах и список процессов
        private async Task RefreshAllDataAsync()
        {
            try
            {
                StatusMessage = "Обновление данных...";

                var cpuTask = Task.Run(() => _cpuMonitor.GetCpuInfo());
                var memoryTask = Task.Run(() => _memoryMonitor.GetMemoryInfo());
                var diskTask = Task.Run(() => _diskMonitor.GetDiskInfo());
                var gpuTask = Task.Run(() => _gpuMonitor.GetGpuInfo());
                var networkTask = Task.Run(() => _networkMonitor.GetNetworkInfo());
                var systemTask = Task.Run(() => _systemMonitor.GetSystemInfo());

                await Task.WhenAll(cpuTask, memoryTask, diskTask, gpuTask, networkTask, systemTask);

                CpuInfo = await cpuTask;
                MemoryInfo = await memoryTask;
                DiskInfo = await diskTask;
                GpuInfo = await gpuTask;
                NetworkInfo = await networkTask;
                SystemInfo = await systemTask;

                _allProcesses = _processMonitor.GetProcesses()
                    .OrderByDescending(p => p.WorkingSet)
                    .ToList();

                SystemInfo.Processes = _allProcesses.Take(20).ToList();

                OnPropertyChanged(nameof(FilteredProcesses));
                OnPropertyChanged(nameof(FilteredProcessesCount));
                OnPropertyChanged(nameof(TotalProcessesCount));
                OnPropertyChanged(nameof(TotalMemoryUsage));
                OnPropertyChanged(nameof(TotalMemoryUsageMb));

                StatusMessage = $"Данные обновлены: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка обновления: {ex.Message}";
            }
        }

        // Обновляет список процессов вручную
        private void RefreshTaskManager()
        {
            _allProcesses = _processMonitor.GetProcesses()
                .OrderByDescending(p => p.WorkingSet)
                .ToList();

            OnPropertyChanged(nameof(FilteredProcesses));
            OnPropertyChanged(nameof(FilteredProcessesCount));
            OnPropertyChanged(nameof(TotalProcessesCount));
            OnPropertyChanged(nameof(TotalMemoryUsage));
            OnPropertyChanged(nameof(TotalMemoryUsageMb));

            StatusMessage = "Список процессов обновлен";
        }

        // Показывает секцию с информацией о процессоре
        private void ShowCpuSection()
        {
            IsCpuVisible = Visibility.Visible;
            IsMemoryVisible = Visibility.Collapsed;
            IsDiskVisible = Visibility.Collapsed;
            IsGpuVisible = Visibility.Collapsed;
            IsNetworkVisible = Visibility.Collapsed;
            IsTaskManagerVisible = Visibility.Collapsed;
            StatusMessage = "Просмотр информации о процессоре";
        }

        // Показывает секцию с информацией о памяти
        private void ShowMemorySection()
        {
            IsCpuVisible = Visibility.Collapsed;
            IsMemoryVisible = Visibility.Visible;
            IsDiskVisible = Visibility.Collapsed;
            IsGpuVisible = Visibility.Collapsed;
            IsNetworkVisible = Visibility.Collapsed;
            IsTaskManagerVisible = Visibility.Collapsed;
            StatusMessage = "Просмотр информации о памяти";
        }

        // Показывает секцию с информацией о дисках
        private void ShowDiskSection()
        {
            IsCpuVisible = Visibility.Collapsed;
            IsMemoryVisible = Visibility.Collapsed;
            IsDiskVisible = Visibility.Visible;
            IsGpuVisible = Visibility.Collapsed;
            IsNetworkVisible = Visibility.Collapsed;
            IsTaskManagerVisible = Visibility.Collapsed;
            StatusMessage = "Просмотр информации о дисках";
        }

        // Показывает секцию с информацией о видеокарте
        private void ShowGpuSection()
        {
            IsCpuVisible = Visibility.Collapsed;
            IsMemoryVisible = Visibility.Collapsed;
            IsDiskVisible = Visibility.Collapsed;
            IsGpuVisible = Visibility.Visible;
            IsNetworkVisible = Visibility.Collapsed;
            IsTaskManagerVisible = Visibility.Collapsed;
            StatusMessage = "Просмотр информации о видеокарте";
        }

        // Показывает секцию с информацией о сети
        private void ShowNetworkSection()
        {
            IsCpuVisible = Visibility.Collapsed;
            IsMemoryVisible = Visibility.Collapsed;
            IsDiskVisible = Visibility.Collapsed;
            IsGpuVisible = Visibility.Collapsed;
            IsNetworkVisible = Visibility.Visible;
            IsTaskManagerVisible = Visibility.Collapsed;
            StatusMessage = "Просмотр информации о сети";
        }

        // Показывает секцию диспетчера задач со списком процессов
        private void ShowTaskManagerSection()
        {
            IsCpuVisible = Visibility.Collapsed;
            IsMemoryVisible = Visibility.Collapsed;
            IsDiskVisible = Visibility.Collapsed;
            IsGpuVisible = Visibility.Collapsed;
            IsNetworkVisible = Visibility.Collapsed;
            IsTaskManagerVisible = Visibility.Visible;
            StatusMessage = "Просмотр диспетчера задач";

            RefreshTaskManager();
        }

        // Показывает окно справки с инструкцией по использованию
        private void ShowHelp()
        {
            string helpText =
                "РУКОВОДСТВО ПО ИСПОЛЬЗОВАНИЮ\n" +
                "==============================\n\n" +
                "1. ОСНОВНЫЕ ФУНКЦИИ:\n" +
                "   - Данные обновляются автоматически каждые 3 секунды\n" +
                "   - Кнопка 'Обновить' - ручное обновление всех данных\n\n" +
                "2. ПРОСМОТР КОМПОНЕНТОВ:\n" +
                "   - Нажмите на кнопки компонентов (CPU, GPU, RAM и т.д.)\n" +
                "   - Каждая кнопка показывает детальную информацию\n" +
                "   - Системная информация и процессы видны всегда\n\n" +
                "3. ОТОБРАЖАЕМЫЕ ДАННЫЕ:\n" +
                "   - Процессор: модель, ядра, потоки, частота, загрузка\n" +
                "   - Память: общий объем, доступно, модули, использование\n" +
                "   - Диски: физические и логические диски, заполнение\n" +
                "   - Видеокарта: модель, объем видеопамяти, загрузка\n" +
                "   - Сеть: адаптер, MAC-адрес, скорость\n\n" +
                "4. ДИСПЕТЧЕР ЗАДАЧ:\n" +
                "   - Показывает все активные процессы\n" +
                "   - Можно искать процессы по имени\n" +
                "   - Отображает использование памяти в байтах и МБ\n\n" +
                "5. УПРАВЛЕНИЕ ОБНОВЛЕНИЕМ:\n" +
                "   - Можно изменить интервал обновления\n" +
                "   - Кнопка 'Стоп/Старт' для паузы обновления\n\n" +
                "6. ЭКСПОРТ ДАННЫХ:\n" +
                "   - Экспорт в TXT, CSV или JSON формат\n\n" +
                "7. ПРИМЕЧАНИЯ:\n" +
                "   - Программа использует WMI для сбора информации\n" +
                "   - Для полной работы требуются права администратора";

            MessageBox.Show(helpText, "Справка",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Применяет новый интервал автоматического обновления
        private void ApplyInterval()
        {
            if (_autoRefreshTimer != null && UpdateInterval > 0)
            {
                _autoRefreshTimer.Interval = UpdateInterval * 1000;
                UpdateStatusText = $"Обновление каждые {UpdateInterval} сек";
                StatusMessage = $"Интервал обновления изменен на {UpdateInterval} сек";
            }
        }

        // Включает/выключает автоматическое обновление данных
        private void ToggleUpdate()
        {
            if (_isUpdating)
            {
                _autoRefreshTimer.Stop();
                UpdateButtonText = "Старт";
                UpdateButtonColor = "#4CAF50";
                UpdateStatusText = "Обновление остановлено";
                StatusMessage = "Автообновление остановлено";
            }
            else
            {
                _autoRefreshTimer.Start();
                UpdateButtonText = "Стоп";
                UpdateButtonColor = "#F44336";
                UpdateStatusText = $"Обновление каждые {UpdateInterval} сек";
                StatusMessage = "Автообновление запущено";
            }
            _isUpdating = !_isUpdating;
        }

        // Экспортирует данные в текстовый файл
        private void ExportTxt()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FileName = $"HardwareMonitor_{DateTime.Now:yyyyMMdd_HHmmss}";
            dialog.DefaultExt = "txt";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var exportService = new HardWareMonitor.Utils.ExportService();
                    exportService.ExportToTxt(dialog.FileName, this);
                    StatusMessage = $"Данные экспортированы в {System.IO.Path.GetFileName(dialog.FileName)}";
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Ошибка экспорта: {ex.Message}";
                }
            }
        }

        // Экспортирует данные в CSV файл
        private void ExportCsv()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            dialog.FileName = $"HardwareMonitor_{DateTime.Now:yyyyMMdd_HHmmss}";
            dialog.DefaultExt = "csv";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var exportService = new HardWareMonitor.Utils.ExportService();
                    exportService.ExportToCsv(dialog.FileName, this);
                    StatusMessage = $"Данные экспортированы в {System.IO.Path.GetFileName(dialog.FileName)}";
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Ошибка экспорта: {ex.Message}";
                }
            }
        }

        // Экспортирует данные в JSON файл
        private void ExportJson()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            dialog.FileName = $"HardwareMonitor_{DateTime.Now:yyyyMMdd_HHmmss}";
            dialog.DefaultExt = "json";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var exportService = new HardWareMonitor.Utils.ExportService();
                    exportService.ExportToJson(dialog.FileName, this);
                    StatusMessage = $"Данные экспортированы в {System.IO.Path.GetFileName(dialog.FileName)}";
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Ошибка экспорта: {ex.Message}";
                }
            }
        }
    }
}