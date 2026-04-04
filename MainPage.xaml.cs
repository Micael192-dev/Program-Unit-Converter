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

	private string ConvertNumber(string typeOrigin, string typeDestination)
	{
		string result;
		double NumberInput=double.Parse(EntryNumber.Text);

		Dictionary<string,double> OutputResult=new(){
			{"farenheit-celsius",(NumberInput-32)/9*5},
			{"celsius-farenheit",NumberInput/5*9+32},
			{"kelvin-celsius",(NumberInput-273)/5*5},
			{"celsius-kelvin",NumberInput/5+273},
			{"kelvin-farenheit",(NumberInput-273)/5*9+32},
			{"farenheit-kelvin",(NumberInput-32)/9*5+273},
			{"kilogram-pound",NumberInput/453.6},
			{"pound-kilogram",NumberInput*453.6},
			{"meter-mile",NumberInput/1609.344},
			{"mile-meter",NumberInput*1609.344}
			};

		double valueOutput=OutputResult[$"{typeOrigin}-{typeDestination}"];

		if (valueOutput - (int)valueOutput == 0)
		{
			int valueOutputInteger=(int)valueOutput;
			result=valueOutputInteger.ToString();
		}
		else
		{
			result=valueOutput.ToString();
		}

		return result;
	}

	private void OnClickedConvert(object? sender, EventArgs e)
	{
		string? typeOrigin="";
		string? typeDestination="";

		if (PickerOrigin.SelectedItem!=null && PickerDestination.SelectedItem !=null)
		{
			typeOrigin=PickerOrigin.SelectedItem.ToString();
			typeDestination=PickerDestination.SelectedItem.ToString();
		}

		bool InputNumberValid=double.TryParse(EntryNumber.Text, out double _);

		if (!InputNumberValid)
		{
			LabelResult.Text="";
		}

		if (typeOrigin!=typeDestination && typeOrigin!=null && typeDestination!=null
		 	&& EntryNumber.Text != null && EntryNumber.Text != "" && InputNumberValid)
		{
			LabelResult.Text=ConvertNumber(typeOrigin,typeDestination);

			string? Category=PickerCategory.SelectedItem.ToString();

			if (Category == "Temperature")
			{
				ChangeBrackgroundColorWithTemperature(typeDestination);
			}	

			if (LastConversions.Count > 4)
			{
				LastConversions.RemoveAt(0);
			}

			if (Category!=null)
			{
				UpdateCollectionLastConversions(Category,EntryNumber.Text,typeOrigin,typeDestination,LabelResult.Text);
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

		if (ItemSelectedPickerCategory == "Temperature")
		{
			OptionsConvertion.Add("celsius");
			OptionsConvertion.Add("farenheit");
			OptionsConvertion.Add("kelvin");
		}
		else
		{
			BackgroundColor=Colors.Transparent;
		}
	}

    private void ButtonInvertConvertion_Clicked(object sender, EventArgs e)
	{
		object typeCurrentPickerOrigin=PickerOrigin.SelectedItem;
		object typeCurrentPickerDestination=PickerDestination.SelectedItem;

		PickerOrigin.SelectedItem=typeCurrentPickerDestination;
		PickerDestination.SelectedItem=typeCurrentPickerOrigin;
	}

	private void ChangeBrackgroundColorWithTemperature(string typeDestination)
	{
		double temperatureConvertForCelsius;
		if (typeDestination != "celsius")
		{
			temperatureConvertForCelsius=double.Parse(ConvertNumber(typeDestination,"celsius"));
		}
		else
		{
			temperatureConvertForCelsius=double.Parse(EntryNumber.Text);
		}

		if(temperatureConvertForCelsius<=0)
		{
			BackgroundColor=Colors.DarkBlue;
		}
		else if(temperatureConvertForCelsius>0 && temperatureConvertForCelsius<40)
		{
			BackgroundColor=Colors.DarkOrange;
		}
		else if(temperatureConvertForCelsius>40)
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

			if (PickerCategory.SelectedItem.ToString() == "Temperature")
			{
				ChangeBrackgroundColorWithTemperature(PickerDestination.SelectedItem.ToString()!);
			}
			else
			{
				BackgroundColor=Colors.Transparent;
			}
		}
	}
}