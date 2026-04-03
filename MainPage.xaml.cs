using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
	public ObservableCollection<string> OptionsConvertion=new();
	public ObservableCollection<string> LastConversions=new();
	public Dictionary<string,string[]> ConfigConvertion=new();

	public MainPage()
	{
		InitializeComponent();

		PickerOrigin.ItemsSource=OptionsConvertion;
		PickerDestination.ItemsSource=OptionsConvertion;
		PickerLastConversions.ItemsSource=LastConversions;
	}

	private int ConvertNumber(string typeOrigin, string typeDestination)
	{
		int result;
		int NumberInput=int.Parse(EntryNumber.Text);

		Dictionary<string,double> OutputResult=new(){
			{"farenheit-celsius",(NumberInput-32)/9*5},
			{"celsius-farenheit",NumberInput/5*9+32},
			{"kelvin-celsius",(NumberInput-273)/5*5},
			{"celsius-kelvin",NumberInput/5+273},
			{"kelvin-farenheit",(NumberInput-273)/5*9+32},
			{"farenheit-kelvin",(NumberInput-32)/9*5+273},
			{"kilogram-pound",NumberInput/453.6},
			{"pound-kilogram",NumberInput*453.6},
			{"meter-mile",NumberInput*0.00062137},
			{"mile-meter",NumberInput/0.00062137}
			};

		result=(int)OutputResult[$"{typeOrigin}-{typeDestination}"];

		return result;
	}

	private void OnClickedConvert(object? sender, EventArgs e)
	{
		int result=0;
		string? typeOrigin="";
		string? typeDestination="";

		if (PickerOrigin.SelectedItem!=null && PickerDestination.SelectedItem !=null)
		{
			typeOrigin=PickerOrigin.SelectedItem.ToString();
			typeDestination=PickerDestination.SelectedItem.ToString();
		}

		if (typeOrigin!=typeDestination && typeOrigin!=null && typeDestination!=null
		 	&& EntryNumber.Text != null && EntryNumber.Text != "")
		{
			result=ConvertNumber(typeOrigin,typeDestination);
			LabelResult.Text=result.ToString();

			string? Category=PickerCategory.SelectedItem.ToString();

			if (Category == "Temperature")
			{
				ChangeBrackgroundColorWithTemperature(result,typeDestination);
			}	
			else
			{
				BackgroundColor=Colors.Transparent;
			}

			if (LastConversions.Count > 4)
			{
				LastConversions.RemoveAt(0);
			}

			if (Category!=null)
			{
				UpdateCollectionLastConversions(Category,EntryNumber.Text,typeOrigin,typeDestination,result.ToString());
			}
		}
	}

	private void OnConfigOptionsConvertion(object? sender=null, EventArgs? e=null)
	{
		string ItemSelectedPickerCategory="";

		if (PickerCategory.SelectedItem!=null)
		{
			ItemSelectedPickerCategory=PickerCategory.SelectedItem.ToString();
			OptionsConvertion.Clear();
		}	
		
		if (ItemSelectedPickerCategory == "Weight")
		{
			OptionsConvertion.Add("kilogram");
			OptionsConvertion.Add("pound");
		}

		else if (ItemSelectedPickerCategory == "Lenght")
		{
			OptionsConvertion.Add("meter");
			OptionsConvertion.Add("mile");
		}

		else if (ItemSelectedPickerCategory == "Temperature")
		{
			OptionsConvertion.Add("celsius");
			OptionsConvertion.Add("farenheit");
			OptionsConvertion.Add("kelvin");
		}
	}

    private void ButtonInvertConvertion_Clicked(object sender, EventArgs e)
	{
		object typeCurrentPickerOrigin=PickerOrigin.SelectedItem;
		object typeCurrentPickerDestination=PickerDestination.SelectedItem;

		PickerOrigin.SelectedItem=typeCurrentPickerDestination;
		PickerDestination.SelectedItem=typeCurrentPickerOrigin;
	}

	private void ChangeBrackgroundColorWithTemperature(int temperature,string typeDestination)
	{
		int temperatureConvertForCelsius;
		if (typeDestination != "celsius")
		{
			temperatureConvertForCelsius=ConvertNumber(typeDestination,"celsius");
		}
		else
		{
			temperatureConvertForCelsius=temperature;
		}

		if(temperatureConvertForCelsius<=0)
		{
			BackgroundColor=Colors.DarkBlue;
		}
		else if(temperatureConvertForCelsius>0 && temperatureConvertForCelsius<40)
		{
			BackgroundColor=Colors.DarkOrange;
		}
		else if(temperature>40)
		{
			BackgroundColor=Colors.DarkRed;
		}
	}

	private void UpdateCollectionLastConversions(string Category,string InputNumber, string typeOrigin, 
												 string typeDestination, string Result)
	{
		string KeyConvertion=$"{Result} ({typeOrigin} -> {typeDestination})";
		string[]configuration={Category,InputNumber,typeOrigin,typeDestination,Result};

		ConfigConvertion[KeyConvertion]=configuration;
		LastConversions.Add(KeyConvertion);
	}

    private void LoadConfigurationLastConvertionSelected(object sender, FocusEventArgs e)
	{
		string? LastConvertionSelected=null;

		if (PickerLastConversions.SelectedItem != null)
		{
			LastConvertionSelected=PickerLastConversions.SelectedItem.ToString();
		}

		if (LastConvertionSelected != null)
		{
			string[] ConfigurationLastConvertion=ConfigConvertion[LastConvertionSelected];

			PickerCategory.SelectedItem=ConfigurationLastConvertion[0];
			OnConfigOptionsConvertion();
			EntryNumber.Text=ConfigurationLastConvertion[1];
			PickerOrigin.SelectedItem=ConfigurationLastConvertion[2];
			PickerDestination.SelectedItem=ConfigurationLastConvertion[3];
			LabelResult.Text=ConfigurationLastConvertion[4];
		}
	}
}