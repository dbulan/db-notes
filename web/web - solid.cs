# WEB - SOLID

Becoming a better developer by using the SOLID design principles by Katerina Trajchevska | https://www.youtube.com/watch?v=rtmFCcjEgEw

/** The purpose of SOLID design principles */
- To make the code more maintable.
- To make it easier to quickly extend the system with new functionallity without breaking the existing ones.
- To make the code easier to read and understand, thus spend less time figuring out what it does and more time actually developing the solution.


# [S] - Single Responsibility Principle
-
/** A class should have one, and only one, reason to change. */
- A class should only be responsible for one thing.
- There's a place for everything and everything is in its place.
- Find one reason to change and take everything else out of the class.
- Very precise names for many small classes > generic names for large classes.

// Bad //
public function store(Request $request, User $user)
{
	$validateData = $request->validate(['name' => 'required']);
	
	$user->name = $request->name;
	$user->save();
	
	return response()->json(['user' => $user], 201);
}

// Controller should control the flow of the app
- Gets input from the client
- Operate with input, call some methods
- Return the output

// Good //
public function store(StoreUserRequest $request, UserRepository $userRepository)
{
	$user = $userRepository->create($request);
	
	return response()->json(['user' => $user], 201);
}
class StoreUserRequest extends FormRequest
{
	public function authorize { return true; }
	public function rules() { return ['name' => 'required']; }
}

class UserRepository
{
	public function create($userData)
	{
		$user = new User();
		$user->name = $userData->name;
		$user->save();
		$user->save();
	
		return $user;
	}
}


# [O] - Open/Closed Principle
-
/** An entity should be open for extension, but closed for modification. */
- Extend functionality by adding new code instead of changing existing code.
- Separate the behaviours, so the system can easily be extended, but never broken.
- Goal: get to a point where you can never break the core of your system.

// Bad //
public function pay(Request $request)
{
	$payment = new Payment();
	
	$type === 'credit' ? $payment->payWithCreditCard() : $payment->payWithPaypal();
	// How to extend by another Wire/Stripe/Bcoin method? With if-else/switch ?
}
class Payment()
{
	public function payWithCreditCard() {}
	public function payWithPaypal() {}
}
 
// Good //
interface PayableInterface
{
	public function pay();
}
class CreditCardPayment implements PayableInterface
{
	public function pay() { ... }
}
class CreditCardPayment implements PayableInterface
{
	public function pay() { ... }
}

class PaymentFactory
{
	public function initPayment($type)
	{
		if($type == 'credit') return new CreditCardPayment();
		else
		if($type == 'paypal') return new PaypalPayment();
	
		throw new Exception("Unsupported payment method");
	}
}

public function pay(Request $request)
{
	$paymentFactory = new PaymentFactory();
	$payment = $paymentFactory->initPayment($request->type);
	$payment->pay();
}


# [L] - Liskov Substitution Principle
-
/** */
- Any derived class should be able to substitute its parent class without the consumer knowing it.
- Every class that implements an interface, must be able to substitute any reference throughout the code that implements that same interface.
- Every part of the code should get the expected result no matter what instance of a class you send to it, given it implements the same interface.

// Bad //
class RubberDuck extends Duck
{
	public function quack() { $person->squeezeDuck($this) ?  'quacking' : throw new Exception; }
	public function fly()   { throw new Exception; }
	public function swim()  { $person->putInTub($this) ? 'swimming' : throw new Exception; }
}

// Good //
interface QuackableInterface { public function quack() : string; }
interface FlyableInterface   { public function fly()   : string; }
interface SwimmableInterface { public function swim()  : string; }

class RubberDuck implements QuackableInterface, SwimmableInterface
{
	public function quack() : string { ... }
	public function swim()  : string { ... }
}


# [I] - Interface Segregation Principle
-
/** No client should be forced to depend on methods it does not use */
- A client should never be forced to depend on methods it doesn't use.
- Or, a client should never depend on anything more than the method it's calling.
- Changing one methods in a class shouldn't affect classes that don't depend on it.
- Replace fat interfaces with many small, specific interfaces.

// Bad //
class Subscriber extends Model
{
	public function subscribe() { }
	public function unsubscribe() { }
	public function getNotifyEmail() { }
}

class Notifications
{
	public function send(Subscriber $subscriber, $message)
	{
		Mail::to($subscriber->getNotifyEmail())->queue($message);
	}
}

// Good //
interface NotifiableInterface
{
	public function getNotifyEmail() : string;
}
class Notifications
{
	public function send(NotifiableInterface $subscriber, $message)
	{
		Mail::to($subscriber->getNotifyEmail())->queue($message);
	}
}


# [D] - Dependency Inversion Principle
-
/** High-level modules should not depend on  low-level modules. Both should depend on abstractions */
- Never depend on anything concrete, only depend on abstractions.
- Able to change an implementation easily without altering the high level code.

// Bad //
public function index(User $user)
{
	$users = $user->where('created_at', '>=', Carbon::yesterday())->get();
	return response()->json(compact('users'), 200);
}

// Good //
class UserRepository implements UserRepositoryInterface
{
	public function getAfterDate(Carbon $date) { return User::where('created_at', '>=', $date); }
	public function create($userData) 
	{
		$user = new User();
		$user->name = $userData->name;
		$user->save();
		
		return $user;
	}
}
interface UserRepositoryInterface
{
	public function getAfterDate(Carbon $date);
	public function create (object $user);
}

public function index(UserRepositoryInterface $user)
{
	$users = $user->getAfterDate(Carbon::yesterday());
	return response()->json(compact('users'), 200);
}