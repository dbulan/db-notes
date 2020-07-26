# LARAVEL - DESIGN PATTERNS

Bobby Bouwmann - Laravel Design Patterns - Laracon EU 2017 | https://www.youtube.com/watch?v=mNl4cMLlplM


/** Why use Desing Patterns */
- Provide a way to solve issues related to software development using a proven solution.
- Makes communication between developer more efficient.


# Design Patterns types
-
1. Creational Patterns
	- Class instantiation
	- Hiding the creation logic
	- Examples: Factory, Builder, Singleton
	
2. Structural Patterns
	- Composition between objects
	- Usages of interfaces
	- Examples: Adapter, Decorator, Facade, Proxy
	
3. Behavioural Patterns
	- Communication between objects
	- Usages of interfaces
	- Examples: Command, Iterator, Observer, Strategy
	

# Factory Patterns
-
/** Provides an interface for creating objects with specifying their concrete classes */
- Factory is responsible for creating objects, not the client
- Multiple clients call the same factory, one place for changes
- Easier to test, easy to mock and isolate

// Example //
interface PizzaFactoryContract
{
	public function make(array $toppings = []) : Pizza;
}
class PizzaFactory implements PizzaFactoryContract
{
	public function make(array $toppings = [])
	{
		return new Pizza($toppings);
	}
}
$pizza = (new PizzaFactory)->make(['chicken', 'onion']);


# Builder (Manager) Pattern
-
/** Builds complex objects step by step, it can return different objects based on the given data */
- Focuses on building complex objects step by step and returns them
- Has functionality to decide which objects should be returned

// Example //
class PizzaBuilder
{
	public function make(PizzaBuilderInterface $pizza) : Pizza
	{
		$pizza->prepare();
		$pizza->applyToppings();
		$pizza->bake();
		
		return $pizza;
	}
}
interface PizzaBuilderInterface
{
	public function prepare();
	public function applyToppings();
	public function bake();
}

class MargarithaBuilder implements PizzaBuilderInterface
{
	protected $pizza;
	
	public function prepare() : Pizza
	{
		$this->pizza = new Pizza();
		
		return $this->pizza;
	}
	
	public function applyToppings() : Pizza
	{
		$this->pizza->setToppings(['cheese', 'tomato']);
		
		return $this->pizza;
	}
	
	public function bake() : Pizza
	{
		$this->pizza->setBakingTemperature(180);
		$this->pizza->setBakingMinutes(8);
		
		return $this->pizza;
	}
}

$pizzaBuilder = new PizzaBuilder;
$pizzaOne = $pizzaBuilder->make(new MargarithaBuilder());
$pizzaTwo = $pizzaBuilder->make(new ChickenBuilder());


# Strategy Pattern
-
/** Defines a family of algorithms that are interchangeable */
/** Program to an interface, not an implementation */

interface DeliveryStrategy
{
	public function deliver(Address $address): DeliveryTime;
}
class BikeDelivery implements DeliveryStrategy
{
	public function deliver(Address $address):
	{
		$route = new BikeRoute($address);
		
		echo $route->calculateCosts();
		echo $route->calculateDeliveryTime();
	}
}
class DroneDelivery implements DeliveryStrategy
{
	public function deliver(Address $address):
	{
		$route = new DroneRoute($address);
		
		echo $route->calculateCosts();
		echo $route->calculateFlyTime();
	}
}

class PizzaDelivery
{
	public function deliverPizza(DeliveryStrategy $strategy, Address $address)
	{
		return $strategy->deliver($address);
	}
}

$address = new Addres('Rigas street');
$delivery = new PizzaDelivery();

$delivery->deliver(new BikeDelivery(), $address);
$delivery->deliver(new DroneDelivery(), $address);