# Lottas Flea Market

## UML

### Sequence Diagram
![Sequence Diagram](Docs/SequenceDiagram.png)

The sequence diagram describes the flow of interactions between the different participants.
The Buyer might start by registering itself as an observer with the market so that whenever a seller puts
an item up for sale the market will notify the buyer of the new item so it can act upon it.
However if a buyer registers itself with the market after there is already items on sale, the market will immediately 
notify the buyer with all existing items, so it can act upon it. 
When the buyer is notified of items it will determine if it has enough money to buy the item.
If the buyer want to buy an item it will contact the seller which in turn will remove the item from the market.

### Use Case Diagram
![Use Case Diagram](Docs/UseCaseDiagram.png)

The use case diagram describes two actors; a seller and a buyer both of which has their own use cases.
The application Market describes 3 different use cases; Putting an item up for sale and remove item 
from market which is used by the Seller, and Looking for new items which is used by the Buyer.

### Class Diagram
![Class Diagram](Docs/ClassDiagram.png)
